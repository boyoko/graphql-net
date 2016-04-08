﻿//MIT License
//
//Copyright (c) 2016 Robert Peele
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace GraphQL.Parser.CS
open GraphQL.Parser
open System.Collections.Generic

// This module adds abstract classes suitable as a starting point for implementing
// a schema from a C# project.

[<AbstractClass>]
type SchemaCS<'s>() =
    abstract member ResolveVariableType : name : string -> ISchemaVariableType
    default this.ResolveVariableType(_) = Unchecked.defaultof<_>
    abstract member ResolveQueryType : name : string -> ISchemaQueryType<'s>
    abstract member ResolveEnumValue : name : string -> EnumValue
    default this.ResolveEnumValue(_) = Unchecked.defaultof<_>
    abstract member ResolveDirective : name : string -> ISchemaDirective<'s>
    default this.ResolveDirective(_) = Unchecked.defaultof<_>
    abstract member RootType : ISchemaQueryType<'s>
    interface ISchema<'s> with
        member this.ResolveDirectiveByName(name) =
            this.ResolveDirective(name) |> obj2option
        member this.ResolveVariableTypeByName(name) =
            this.ResolveVariableType(name) |> obj2option
        member this.ResolveQueryTypeByName(name) =
            this.ResolveQueryType(name) |> obj2option
        member this.ResolveEnumValueByName(name) =
            this.ResolveEnumValue(name) |> obj2option
        member this.RootType = this.RootType

[<AbstractClass>]
type SchemaQueryTypeCS<'s>() =
    abstract member TypeName : string
    abstract member Description : string
    default this.Description = null
    abstract member Info : 's
    default this.Info = Unchecked.defaultof<'s>
    abstract member Fields : IReadOnlyDictionary<string, ISchemaField<'s>>
    interface ISchemaQueryType<'s> with
        member this.TypeName = this.TypeName
        member this.Description = this.Description |> obj2option
        member this.Info = this.Info
        member this.Fields = this.Fields

[<AbstractClass>]
type SchemaQueryableFieldCS<'s>() =
    abstract member DeclaringType : ISchemaQueryType<'s>
    abstract member FieldType : ISchemaQueryType<'s>
    abstract member FieldName : string
    abstract member Description : string
    default this.Description = null
    abstract member Info : 's
    default this.Info = Unchecked.defaultof<'s>
    abstract member Arguments : IReadOnlyDictionary<string, ISchemaArgument<'s>>
    default this.Arguments = emptyDictionary
    interface ISchemaField<'s> with
        member this.DeclaringType = this.DeclaringType
        member this.FieldType = QueryField this.FieldType
        member this.FieldName = this.FieldName
        member this.Description = this.Description |> obj2option
        member this.Info = this.Info
        member this.Arguments = this.Arguments

[<AbstractClass>]
type SchemaValueFieldCS<'s>() =
    abstract member DeclaringType : ISchemaQueryType<'s>
    abstract member IsNullable : bool
    default this.IsNullable = false
    abstract member FieldType : CoreVariableType
    abstract member FieldName : string
    abstract member Description : string
    default this.Description = null
    abstract member Info : 's
    default this.Info = Unchecked.defaultof<'s>
    abstract member Arguments : IReadOnlyDictionary<string, ISchemaArgument<'s>>
    default this.Arguments = emptyDictionary
    static member Integer = PrimitiveType IntType
    static member Float = PrimitiveType FloatType
    static member String = PrimitiveType StringType
    static member Boolean = PrimitiveType BooleanType
    interface ISchemaField<'s> with
        member this.DeclaringType = this.DeclaringType
        member this.FieldType = ValueField { Nullable = this.IsNullable; Type = this.FieldType }
        member this.FieldName = this.FieldName
        member this.Description = this.Description |> obj2option
        member this.Info = this.Info
        member this.Arguments = this.Arguments


        
