import ldap = require('ldapjs');

interface DirectoryEntry {
    dn: string;
    samaccountname?: string;
    attributes: any;
}

class MockLdapInstance {
    directory: DirectoryEntry[];
    ldapServer: any;
    readonly tld: string = "o=example";
    readonly user: string = "neoman";
    readonly mailDomain: string = "example.com";

    constructor() {
        this.directory = this.buildDirectory();
    }

    start(port: number, fakeLatency?: number) {
        this.ldapServer = ldap["createServer"]();
        this.ldapServer.on('bind', function(bindEvent) {
            console.log(`Bind Event: Success - ${bindEvent.success}, dn: ${bindEvent.dn}, credentials: ${bindEvent.credentials}`);
        });
        this.ldapServer.on('authorize', function(e) {
            console.log(`Auth ${e.success ? 'ok' : 'fail'}, dn: ${e.dn}`);
        });
        this.ldapServer.on('listening', function() { 
            console.log("LDAP SERVER LISTENING");
        });
        this.ldapServer.on('close', function() { 
            console.log("LDAP SERVER STOPPED LISTENING");
        });
        this.ldapServer.bind('o=example', function(req, res, next) {
            if (fakeLatency == -1 ) return;
    
            setTimeout(function() {
                this.bindHandler.call(this.ldapServer, this, req, res, next);
            }.bind(this), fakeLatency);
        }.bind(this));

        this.ldapServer.add('ou=users, o=example', function(req, res, next) {
            console.log('DN: ' + req.dn.toString());
            console.log('Entry attributes: ' + req.toObject().attributes);
            res.end();
        });
    
        this.ldapServer.search('o=example', function(req, res, next) {
            if (fakeLatency == -1 ) return;
    
            setTimeout(function() {
                this.searchHandler.call(this.ldapServer, this, req, res, next);
            }.bind(this), fakeLatency);
        }.bind(this));

        this.ldapServer.listen(port, '127.0.0.1', function() {
            this.ldapServer.emit("listening");
        }.bind(this));
    }

    buildDirectory(): DirectoryEntry[] {
        const dir: DirectoryEntry[] = [];
        dir.push({ dn: `cn=${this.user}, ou=users, ${this.tld}`, attributes: { cn: this.user, password: "testtest" } });
        dir.push({ dn: `mail=user@example.com, ou=users, ${this.tld}`, attributes: { mail: 'user@example.com' }})
        for (let i=0; i<10; i++) {
            dir.push({
                dn: `mail=user${i}@${this.mailDomain}, ou=users, ${this.tld}`,
                samaccountname: `estestsomething${i}`,
                attributes: {
                    samaccountname: `estestsomething${i}`,
                    mail: `user${i}@${this.mailDomain}`,
                    customprop: Math.random(),
                    mailAlias: `user${i}-alias@${this.mailDomain}`,
                    password: `testtest${i}`
                }
            });
        }

        return dir;
    }

    bindHandler(this: any, self: MockLdapInstance, req, res, next) {
        var bindDN = req.dn.toString();
        var credentials = req.credentials;
        for(var i=0; i < self.directory.length; i++) {
          if(self.directory[i].dn === bindDN && 
             credentials === self.directory[i].attributes.password &&
             self.directory[i].attributes.employeetype !== 'DISABLED') {
    
            this.emit('bind', {
              success: true,
              dn: bindDN,
              credentials: credentials
            });
    
            res.end();
            return next();
          }
        }
    
        this.emit('bind', {
          success: false,
          dn: bindDN,
          credentials: credentials
        });
    
        return next(new (<any>ldap).InvalidCredentialsError());
      }
    
      searchHandler(self: MockLdapInstance, req, res, next) {
        self.directory.forEach(function(user) {
          // this test is pretty dumb, make sure in the directory
          // that things are spaced / cased exactly
    
          if (user.dn.indexOf(req.dn.toString()) === -1) {
            return;
          }
    
          if (req.filter.matches(user.attributes)) {
            res.send(user);
          }
        });
    
        res.end();
        return next();
      }
    
      // some middleware to make sure the user has a successfully bind
      authorize(this: any, self: MockLdapInstance, req, res, next) {
        for(var i=0; i < self.directory.length; i++) {
          if (req.connection.ldap.bindDN.equals(self.directory[i].dn)) {
            this.emit('authorize', {
              success: true,
              dn: req.connection.ldap.bindDN
            });
            return next();
          }
        }
    
        this.emit('authorize', {
          success: false,
          dn: req.connection.ldap.bindDN
        });
    
        return next(new (<any>ldap).InsufficientAccessRightsError());
      }
}

const port = parseInt(process.argv[2] || "1389");
new MockLdapInstance().start(port);