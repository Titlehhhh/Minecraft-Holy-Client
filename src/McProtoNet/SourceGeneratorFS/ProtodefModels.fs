namespace SourceGenerator

open System.Collections.Generic

module ProtodefModels =

    type ByteOrder =
        | BigEndian
        | LittleEndian

    and Numeric =
        | Byte of signed: bool
        | Short of signed: bool * byteOrder: ByteOrder
        | Int of signed: bool * byteOrder: ByteOrder
        | Long of signed: bool * byteOrder: ByteOrder
        | Float of byteOrder: ByteOrder
        | Double of byteOrder: ByteOrder
        | VarInt
        | VarLong

    and Primitive =
        | Boolean
        | String
        | Void

    and ArrayCount =
        | FieldReference of string
        | FixedLength of uint32

    and Array = {
        countType: DataType option
        count: ArrayCount option
        elementsType: DataType
    }

    and Field = {
        name: string option
        fieldType: DataType
        anonymous: bool option
    }

    and Count = {
        countType: DataType
        countFor: string
    }

    and Structure =
        | Array of Array
        | Container of Field list
        | Count of Count

    and Buffer = {
        countType: DataType option
        count: ArrayCount option
        rest: bool option
    }

    and Mapper = {
        mappingsType: string
        mappings: Dictionary<string, string>
    }

    and BitField = {
        name: string
        size: int
        signed: bool
    }

    and Loop = {
        endVal: uint32
        dataType: DataType
    }

    and Util =
        | Buffer of Buffer
        | Mapper of Mapper
        | Bitfield of BitField list
        | PrefixedString of countType: DataType
        | Loop of Loop
        | TopBitSetTerminatedArray of Structure
    and DataType =
        | Conditional of Conditional
        | Numeric of Numeric
        | Primitive of Primitive
        | Structure of Structure
        | Util of Util
        | Custom of string

    and Conditional =
        | Switch of Switch
        | Option of DataType

    and Switch = {
        name: string option
        compareTo: string
        fields: Dictionary<string, DataType>
        default_val: DataType option
    }



    and Namespace =
        | Map of Dictionary<string, Namespace>
        | DataType of DataType

    type Protocol = {
        types: Dictionary<string, DataType>
        namespaces: Dictionary<string, Namespace>
    }
