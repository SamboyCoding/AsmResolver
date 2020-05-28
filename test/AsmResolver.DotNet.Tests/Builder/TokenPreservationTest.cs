using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsmResolver.DotNet.Builder;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Xunit;

namespace AsmResolver.DotNet.Tests.Builder
{
    public class TokenPreservationTest
    {
        private static readonly SignatureComparer Comparer = new SignatureComparer();
        
        private static List<TMember> GetMembers<TMember>(ModuleDefinition module, TableIndex tableIndex)
        {
            int count = module.DotNetDirectory.Metadata
                .GetStream<TablesStream>()
                .GetTable(tableIndex)
                .Count;

            var result = new List<TMember>();
            for (uint rid = 1; rid <= count; rid++)
                result.Add((TMember) module.LookupMember(new MetadataToken(tableIndex, rid)));
            return result;
        }

        private static ModuleDefinition RebuildAndReloadModule(ModuleDefinition module, MetadataBuilderFlags metadataBuilderFlags)
        {
            var builder = new ManagedPEImageBuilder
            {
                MetadataBuilderFlags = metadataBuilderFlags
            };
            
            var newImage = builder.CreateImage(module);
            return ModuleDefinition.FromImage(newImage);
        }

        [Fact]
        public void PreserveTypeRefsNoChangeShouldAtLeastHaveOriginalTypeRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeRefs = GetMembers<TypeReference>(module, TableIndex.TypeRef);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeReferenceIndices);
            var newTypeRefs = GetMembers<TypeReference>(newModule, TableIndex.TypeRef);
            
            Assert.Equal(originalTypeRefs, newTypeRefs.Take(originalTypeRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveTypeRefsWithTypeRefRemovedShouldAtLeastHaveOriginalTypeRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeRefs = GetMembers<TypeReference>(module, TableIndex.TypeRef);
            
            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.Clear();
            instructions.Add(new CilInstruction(CilOpCodes.Ret));
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeReferenceIndices);
            var newTypeRefs = GetMembers<TypeReference>(newModule, TableIndex.TypeRef);
            
            Assert.Equal(originalTypeRefs, newTypeRefs.Take(originalTypeRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveTypeRefsWithExtraImportShouldAtLeastHaveOriginalTypeRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeRefs = GetMembers<TypeReference>(module, TableIndex.TypeRef);
            
            var importer = new ReferenceImporter(module);
            var readKey = importer.ImportMethod(typeof(Console).GetMethod("ReadKey", Type.EmptyTypes));

            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.RemoveAt(instructions.Count-1);
            instructions.AddRange(new[]
            {
                new CilInstruction(CilOpCodes.Call, readKey),
                new CilInstruction(CilOpCodes.Pop),
                new CilInstruction(CilOpCodes.Ret),
            });
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeReferenceIndices);
            var newTypeRefs = GetMembers<TypeReference>(newModule, TableIndex.TypeRef);
            
            Assert.Equal(originalTypeRefs, newTypeRefs.Take(originalTypeRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveMemberRefsNoChangeShouldAtLeastHaveOriginalMemberRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalMemberRefs = GetMembers<MemberReference>(module, TableIndex.MemberRef);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveMemberReferenceIndices);
            var newMemberRefs = GetMembers<MemberReference>(newModule, TableIndex.MemberRef);
            
            Assert.Equal(originalMemberRefs, newMemberRefs.Take(originalMemberRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveMemberRefsWithTypeRefRemovedShouldAtLeastHaveOriginalMemberRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalMemberRefs = GetMembers<MemberReference>(module, TableIndex.MemberRef);
            
            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.Clear();
            instructions.Add(new CilInstruction(CilOpCodes.Ret));
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveMemberReferenceIndices);
            var newMemberRefs = GetMembers<MemberReference>(newModule, TableIndex.MemberRef);
            
            Assert.Equal(originalMemberRefs, newMemberRefs.Take(originalMemberRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveMemberRefsWithExtraImportShouldAtLeastHaveOriginalMemberRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalMemberRefs = GetMembers<MemberReference>(module, TableIndex.MemberRef);
            
            var importer = new ReferenceImporter(module);
            var readKey = importer.ImportMethod(typeof(Console).GetMethod("ReadKey", Type.EmptyTypes));

            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.RemoveAt(instructions.Count-1);
            instructions.AddRange(new[]
            {
                new CilInstruction(CilOpCodes.Call, readKey),
                new CilInstruction(CilOpCodes.Pop),
                new CilInstruction(CilOpCodes.Ret),
            });
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveMemberReferenceIndices);
            var newMemberRefs = GetMembers<MemberReference>(newModule, TableIndex.MemberRef);
            
            Assert.Equal(originalMemberRefs, newMemberRefs.Take(originalMemberRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveAssemblyRefsNoChangeShouldAtLeastHaveOriginalAssemblyRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalAssemblyRefs = GetMembers<AssemblyReference>(module, TableIndex.AssemblyRef);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveAssemblyReferenceIndices);
            var newAssemblyRefs = GetMembers<AssemblyReference>(newModule, TableIndex.AssemblyRef);
            
            Assert.Equal(originalAssemblyRefs, newAssemblyRefs.Take(originalAssemblyRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveAssemblyRefsWithTypeRefRemovedShouldAtLeastHaveOriginalAssemblyRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalAssemblyRefs = GetMembers<AssemblyReference>(module, TableIndex.AssemblyRef);
            
            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.Clear();
            instructions.Add(new CilInstruction(CilOpCodes.Ret));
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveAssemblyReferenceIndices);
            var newAssemblyRefs = GetMembers<AssemblyReference>(newModule, TableIndex.AssemblyRef);
            
            Assert.Equal(originalAssemblyRefs, newAssemblyRefs.Take(originalAssemblyRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveAssemblyRefsWithExtraImportShouldAtLeastHaveOriginalAssemblyRefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalAssemblyRefs = GetMembers<AssemblyReference>(module, TableIndex.AssemblyRef);
            
            var importer = new ReferenceImporter(module);
            var exists = importer.ImportMethod(typeof(File).GetMethod("Exists", new[] {typeof(string)}));

            var instructions = module.ManagedEntrypointMethod.CilMethodBody.Instructions;
            instructions.RemoveAt(instructions.Count-1);
            instructions.AddRange(new[]
            {
                new CilInstruction(CilOpCodes.Ldstr, "file.txt"),
                new CilInstruction(CilOpCodes.Call, exists),
                new CilInstruction(CilOpCodes.Pop),
                new CilInstruction(CilOpCodes.Ret),
            });
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveAssemblyReferenceIndices);
            var newAssemblyRefs = GetMembers<AssemblyReference>(newModule, TableIndex.AssemblyRef);
            
            Assert.Equal(originalAssemblyRefs, newAssemblyRefs.Take(originalAssemblyRefs.Count), Comparer);
        }

        [Fact]
        public void PreserveTypeDefsNoChangeShouldResultInSameTypeDefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeDefs = GetMembers<TypeDefinition>(module, TableIndex.TypeDef);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeDefinitionIndices);
            var newTypeDefs = GetMembers<TypeDefinition>(newModule, TableIndex.TypeDef);
            
            Assert.Equal(originalTypeDefs, newTypeDefs.Take(originalTypeDefs.Count), Comparer);
        }

        [Fact]
        public void PreserveTypeDefsAddTypeToEndShouldResultInSameTypeDefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeDefs = GetMembers<TypeDefinition>(module, TableIndex.TypeDef);

            var newType = new TypeDefinition("Namespace", "Name", TypeAttributes.Public);
            module.TopLevelTypes.Add(newType);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeDefinitionIndices);
            var newTypeDefs = GetMembers<TypeDefinition>(newModule, TableIndex.TypeDef);
            
            Assert.Equal(originalTypeDefs, newTypeDefs.Take(originalTypeDefs.Count), Comparer);
            Assert.Contains(newType, newTypeDefs, Comparer);
        }

        [Fact]
        public void PreserveTypeDefsInsertTypeShouldResultInSameTypeDefs()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            var originalTypeDefs = GetMembers<TypeDefinition>(module, TableIndex.TypeDef);
            
            var newType = new TypeDefinition("Namespace", "Name", TypeAttributes.Public);
            module.TopLevelTypes.Insert(1, newType);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeDefinitionIndices);
            var newTypeDefs = GetMembers<TypeDefinition>(newModule, TableIndex.TypeDef);
            
            Assert.Equal(originalTypeDefs, newTypeDefs.Take(originalTypeDefs.Count), Comparer);
            Assert.Contains(newType, newTypeDefs, Comparer);
        }

        [Fact]
        public void PreserveTypeDefsRemoveTypeShouldStuffTypeDefSlots()
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            for (int i = 0; i < 10; i++)
                module.TopLevelTypes.Add(new TypeDefinition("Namespace", $"Type{i.ToString()}", TypeAttributes.Public));
            module = RebuildAndReloadModule(module, MetadataBuilderFlags.None);

            const int indexToRemove = 2;
            
            var originalTypeDefs = GetMembers<TypeDefinition>(module, TableIndex.TypeDef);
            module.TopLevelTypes.RemoveAt(indexToRemove);
            
            var newModule = RebuildAndReloadModule(module, MetadataBuilderFlags.PreserveTypeDefinitionIndices);
            var newTypeDefs = GetMembers<TypeDefinition>(newModule, TableIndex.TypeDef);

            for (int i = 0; i < originalTypeDefs.Count; i++)
            {
                if (i != indexToRemove)
                    Assert.Equal(originalTypeDefs[i], newTypeDefs[i], Comparer);
            }
        }

        private static ModuleDefinition CreateSampleFieldDefsModule(int typeCount, int fieldsPerType)
        {
            var module = ModuleDefinition.FromBytes(Properties.Resources.HelloWorld_NetCore);
            
            for (int i = 0; i < typeCount; i++)
            {
                var dummyType = new TypeDefinition("Namespace", $"Type{i.ToString()}",
                    TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
                
                module.TopLevelTypes.Add(dummyType);
                for (int j = 0; j < fieldsPerType; j++)
                {
                    dummyType.Fields.Add(new FieldDefinition($"Field{j}",
                        FieldAttributes.Public | FieldAttributes.Static,
                        FieldSignature.CreateStatic(module.CorLibTypeFactory.Int32)));
                }
            }

            return RebuildAndReloadModule(module, MetadataBuilderFlags.None);
        }

        private static void AssertSameFieldTokens(ModuleDefinition module, ModuleDefinition newModule, params MetadataToken[] excludeTokens)
        {
            Assert.True(module.TopLevelTypes.Count <= newModule.TopLevelTypes.Count);
            foreach (var originalType in module.TopLevelTypes)
            {
                var newType = newModule.TopLevelTypes.First(t => t.FullName == originalType.FullName);

                Assert.True(originalType.Fields.Count <= newType.Fields.Count);
                foreach (var originalField in originalType.Fields)
                {
                    if (originalField.MetadataToken.Rid == 0 || excludeTokens.Contains(originalField.MetadataToken))
                        continue;
                    
                    var newField = newType.Fields.First(f => f.Name == originalField.Name);
                    Assert.Equal(originalField.MetadataToken, newField.MetadataToken);
                }
            }
        }

        [Fact]
        public void PreserveFieldDefsNoChange()
        {
            var module = CreateSampleFieldDefsModule(10, 10);
            
            var newModule = RebuildAndReloadModule(module,MetadataBuilderFlags.PreserveFieldDefinitionIndices);

            AssertSameFieldTokens(module, newModule);
        }

        [Fact]
        public void PreserveFieldDefsChangeOrderOfTypes()
        {
            var module = CreateSampleFieldDefsModule(10, 10);
            
            const int swapIndex = 3;
            var type = module.TopLevelTypes[swapIndex];
            module.TopLevelTypes.RemoveAt(swapIndex);
            module.TopLevelTypes.Insert(swapIndex + 1, type);
            
            var newModule = RebuildAndReloadModule(module,MetadataBuilderFlags.PreserveFieldDefinitionIndices);

            AssertSameFieldTokens(module, newModule);
        }

        [Fact]
        public void PreserveFieldDefsChangeOrderOfFieldsInType()
        {
            var module = CreateSampleFieldDefsModule(10, 10);

            const int swapIndex = 3;
            var type = module.TopLevelTypes[2];
            var field = type.Fields[swapIndex];
            type.Fields.RemoveAt(swapIndex);
            type.Fields.Insert(swapIndex + 1, field);
            
            var newModule = RebuildAndReloadModule(module,MetadataBuilderFlags.PreserveFieldDefinitionIndices);

            AssertSameFieldTokens(module, newModule);
        }

        [Fact]
        public void PreserveFieldDefsAddExtraField()
        {
            var module = CreateSampleFieldDefsModule(10, 10);

            var type = module.TopLevelTypes[2];
            type.Fields.Insert(3,
                new FieldDefinition("ExtraField", FieldAttributes.Public | FieldAttributes.Static,
                    FieldSignature.CreateStatic(module.CorLibTypeFactory.Int32)));
            
            var newModule = RebuildAndReloadModule(module,MetadataBuilderFlags.PreserveFieldDefinitionIndices);

            AssertSameFieldTokens(module, newModule);
        }

        [Fact]
        public void PreserveFieldDefsRemoveField()
        {
            var module = CreateSampleFieldDefsModule(10, 10);

            var type = module.TopLevelTypes[2];
            const int indexToRemove = 3;
            var field = type.Fields[indexToRemove];
            type.Fields.RemoveAt(indexToRemove);
            
            var newModule = RebuildAndReloadModule(module,MetadataBuilderFlags.PreserveFieldDefinitionIndices);

            AssertSameFieldTokens(module, newModule, field.MetadataToken);
        }
        
        
    }
}