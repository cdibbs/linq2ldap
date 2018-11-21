import ldap = require('ldapjs');

interface Attribute {
    type: string;
    val: any;
}
interface DirectoryEntry {
    dn: string;
    samaccountname?: string;
    attributes: { objectclass: string, [key: string]: string | string[] };
}

function replacer(cache, key, value) {
    if (typeof value === 'object' && value !== null) {
        if (cache.indexOf(value) !== -1) {
            // Duplicate reference found
            try {
                // If this value does not reference a parent it can be deduped
                return JSON.parse(JSON.stringify(value));
            } catch (error) {
                // discard key if value cannot be deduped
                return;
            }
        }
        // Store value in our collection
        cache.push(value);
    }
    return value;
}

class MockLdapInstance {
    directory: DirectoryEntry[];
    ldapServer: any;
    readonly tld: string = "dc=example, dc=com";
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
        this.ldapServer.bind('dc=example, dc=com', function(req, res, next) {
            if (fakeLatency == -1 ) return;
    
            setTimeout(function() {
                this.bindHandler.call(this.ldapServer, this, req, res, next);
            }.bind(this), fakeLatency);
        }.bind(this));

        this.ldapServer.bind('', function(req, res, next) {
            console.log("Invalid bind attempt.");
            console.log(req.baseObject.toString(), req.scope.toString(), req.filter.toString());
            res.end();
            next();
        });

        this.ldapServer.add('dc=example, dc=com', function(req, res, next) {
            console.log('DN: ' + req.dn.toString());
            console.log('Entry attributes: ' + req.toObject().attributes);
            res.end();
        });
    
        this.ldapServer.search('dc=example, dc=com', function(req, res, next) {
            if (fakeLatency == -1 ) return;
    
            setTimeout(function() {
                this.searchHandler.call(this.ldapServer, this, req, res, next);
            }.bind(this), fakeLatency);
        }.bind(this));

        this.ldapServer.search('', function (req, res, next) {
            console.log("\nIncoming search request\n", req.baseObject.toString(), req.scope.toString(), req.filter.toString());
            if (req.scope.toString() === "base" && req.filter.toString() === "(objectclass=*)") {
                console.log("Received root DSE request.");
                // https://ldap.com/dit-and-the-ldap-root-dse/
                var rootDSEEntry = {
                    dn: 'dc=example, dc=com',
                    attributes: {
                        namingContexts: [
                            'dc=example, dc=com'
                        ],
                        subschemaSubentry: 'dc=example, dc=com',
                        supportedLDAPVersion: "3",

                        // https://docs.microsoft.com/en-us/windows/desktop/adschema/rootdse
                        supportedControl: ["1.2.840.113556.1.4.319" /*page */, "1.2.840.113556.1.4.473" /* sort */],

                        supportedSASLMechanisms: [],
                        supportedFeatures: [],
                        dn: `dc=example, dc=com`,
                    }
                };
                console.log("Replying with root DSE...");
                res.send(rootDSEEntry);
                res.end();
                next();
                return;
            }

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
        dir.push({ dn: `cn=${this.user}, ${this.tld}`, attributes: { cn: this.user, password: "testtest", objectclass: "user" } });
        dir.push({ dn: `mail=user@example.com, ${this.tld}`, attributes: { mail: 'user@example.com', objectclass: 'user' }})
        for (let i=0; i<10; i++) {
            dir.push({
                dn: `mail=user${i}@${this.mailDomain}, ${this.tld}`,
                samaccountname: `estestsomething${i}`,
                attributes: {
                    samaccountname: `estestsomething${i}`,
                    mail: `user${i}@${this.mailDomain}`,
                    "alt-mails": [
                        `user${i}-backup-one@example.com`,
                        `user${i}-backup-two@example.com` ],
                    customprop: `${Math.random()}`,
                    mailAlias: `user${i}-alias@${this.mailDomain}`,
                    password: `testtest${i}`,
                    objectclass: 'user'
                }
            });
        }

        return dir;
    }

    bindHandler(this: any, self: MockLdapInstance, req, res, next) {
        var bindDN = req.dn.toString();
        var credentials = req.credentials;
        for(var i=0; i < self.directory.length; i++) {
            console.log(self.directory[i].dn);
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
        console.log(`search: ${req.baseObject} ${req}`);
        var pageCtrl = req.controls.find(c => c.type == "1.2.840.113556.1.4.319");
        var sortCtrl = req.controls.find(c => c.type == "1.2.840.113556.1.4.473");
        var i = 0;
        self.directory.forEach(function (user) {
            if (req.sizeLimit != 0 && i++ > req.sizeLimit) return true;
            if (pageCtrl && i++ > pageCtrl.value.size) return true;
            // this test is pretty dumb, make sure in the directory
            // that things are spaced / cased exactly
            if (user.dn.indexOf(req.baseObject.toString()) === -1) {
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