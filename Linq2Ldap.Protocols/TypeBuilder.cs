using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Proxies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Linq2Ldap.Protocols
{
    internal static class TypeBuilder
    {
        static Type SearchResultEntryProxy;

        static TypeBuilder()
        {
            var assemblyName = new AssemblyName() {
                Name = "DynamicProtocolsProxies"
            };
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var modBuilder = asmBuilder.DefineDynamicModule(asmBuilder.GetName().Name);
            DefineEntryWrapperType(modBuilder);
            /*
            typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Any,
                new [] { typeof() }
                )*/            
        }

        //https://gist.github.com/joeenzminger/7526426
        internal static void DefineEntryWrapperType(ModuleBuilder modBuilder)
        {
            var typeBuilder = modBuilder.DefineType(
                $"Linq{nameof(SearchResultEntry)}",
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                typeof(SearchResultEntry),
                new[] { typeof(IEntry) });

            var ctorTypes = new Type[] { typeof(string), typeof(SearchResultAttributeCollection) };
            var objCtor = typeof(SearchResultEntry).GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, ctorTypes, null);
            var ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, ctorTypes);
            var il = ctorBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Call, objCtor);
            var internalCtorPtr = objCtor.MethodHandle.GetFunctionPointer();
            if (Marshal.SizeOf(internalCtorPtr) == 4)
            {
                il.Emit(OpCodes.Ldc_I4, (int)internalCtorPtr);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I8, (long)internalCtorPtr);
            }
            il.EmitCalli(OpCodes.Calli, objCtor.CallingConvention, null, ctorTypes, null);
            il.Emit(OpCodes.Ret);

            var prop_attrInfo = typeBuilder.DefineProperty(
                nameof(IEntry.Attributes),
                PropertyAttributes.None,
                typeof(DirectoryEntryPropertyCollection),
                Type.EmptyTypes);
            var field_attrInfo = typeBuilder.DefineField(
                "_" + nameof(IEntry.Attributes),
                typeof(DirectoryEntryPropertyCollection),
                FieldAttributes.Private);
            var pGet = typeBuilder.DefineMethod($"get_{prop_attrInfo.Name}", MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, prop_attrInfo.PropertyType, Type.EmptyTypes);
            il = pGet.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field_attrInfo);
            il.Emit(OpCodes.Ret);
            var pSet = typeBuilder.DefineMethod($"set_{prop_attrInfo.Name}", MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { prop_attrInfo.PropertyType });
            il = pSet.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field_attrInfo);
            il.Emit(OpCodes.Ret);
            prop_attrInfo.SetGetMethod(pGet);
            prop_attrInfo.SetSetMethod(pSet);

            var prop_distNameInfo = typeBuilder.DefineProperty(
                nameof(IEntry.DistinguishedName),
                PropertyAttributes.None,
                typeof(string),
                Type.EmptyTypes);

            var baseProp_distNameInfo = typeof(SearchResultEntry)
                .GetProperty(nameof(SearchResultEntry.DistinguishedName), BindingFlags.Public | BindingFlags.Instance);
            var dnPGet = typeBuilder.DefineMethod($"get_{prop_distNameInfo.Name}", MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, prop_distNameInfo.PropertyType, Type.EmptyTypes);
            il = dnPGet.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, baseProp_distNameInfo.GetMethod);
            il.Emit(OpCodes.Ret);
            var dnPSet = typeBuilder.DefineMethod($"set_{prop_distNameInfo.Name}", MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { prop_distNameInfo.PropertyType });
            il = dnPSet.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            var dnInternalSetPtr = baseProp_distNameInfo.GetSetMethod(true).MethodHandle.GetFunctionPointer();
            if (Marshal.SizeOf(dnInternalSetPtr) == 4)
            {
                il.Emit(OpCodes.Ldc_I4, (int)dnInternalSetPtr);
            } else
            {
                il.Emit(OpCodes.Ldc_I8, (long)dnInternalSetPtr);
            }
            il.EmitCalli(OpCodes.Calli, baseProp_distNameInfo.GetSetMethod(true).CallingConvention, null, new[] { typeof(string) }, null);
            il.Emit(OpCodes.Ret);
            prop_distNameInfo.SetGetMethod(dnPGet);
            prop_distNameInfo.SetSetMethod(dnPSet);

            var hasMethodBuilder = typeBuilder.DefineMethod(
                nameof(IEntry.Has),
                MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                CallingConventions.ExplicitThis | CallingConventions.HasThis,
                typeof(bool),
                new[] { typeof(string) });
            il = hasMethodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, pGet);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            var containsKeyMethod = typeof(DirectoryEntryPropertyCollection).GetMethod(nameof(DirectoryEntryPropertyCollection.ContainsKey));
            il.Emit(OpCodes.Callvirt, containsKeyMethod);
            il.Emit(OpCodes.Ret);

            var get_ItemMethodBuilder = typeBuilder.DefineMethod(
                "get_Item",
                MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(PropertyValueCollection),
                new[] { typeof(string) });
            il = get_ItemMethodBuilder.GetILGenerator();
            var attrGetItemMethod = typeof(DirectoryEntryPropertyCollection).GetMethod("get_Item");
            il.Emit(OpCodes.Call, attrGetItemMethod);
            il.Emit(OpCodes.Ret);
            SearchResultEntryProxy = typeBuilder.CreateTypeInfo();
        }

        internal static IEntry CreateLinqSearchResultEntry(string dn, DirectoryEntryPropertyCollection results)
        {
            var entry = (IEntry)Activator.CreateInstance(SearchResultEntryProxy, new object[] { dn, results });
            entry.DistinguishedName = dn;
            entry.Attributes = results;
            return entry;
        }
    }
}
