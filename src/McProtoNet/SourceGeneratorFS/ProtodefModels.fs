module ProtodefModels

type ByteOrder =
    | BigEndian
    | LittleEndian

type Primitive =
    | Boolean
    | String
    | Void


type ArrayCount =
    | FieldReference of string
    | FixedLength of uint32

type Numeric =
    | Byte of signed: bool
    | Short of signed: bool * byteOrder: ByteOrder
    | Int of signed: bool * byteOrder: ByteOrder
    | Long of signed: bool * byteOrder: ByteOrder
    | Float of byteOrder: ByteOrder
    | Double of byteOrder: ByteOrder
    | VarInt of int32
    | VarLong of int64

type BitField =
    { name: string
      size: int
      signed: bool }


type DataType =
    | Conditional of Conditional
    | Numeric of Numeric
    | Primitive of Primitive
    | Structure of Structure
    | Util of Util
    | Custom of string

and Structure =
    | Array of Array
    | Container of Field list
    | Count of Count

and Conditional =
    | Switch of Switch
    | Option of DataType

and Util =
    | Buffer of Buffer
    | Mapper of Mapper
    | Bitfield of BitField list
    | PrefixedString of countType: DataType
    | Loop of Loop
    | TopBitSetTerminatedArray of Structure

and Loop = { endVal: uint32; dataType: DataType }

and Array =
    { countType: DataType option
      count: ArrayCount option
      elementsType: DataType }

and Field =
    { name: string option
      fieldType: DataType
      anonymous: bool option }

and Count =
    { countType: DataType
      countFor: string }



and Buffer =
    { countType: DataType option
      count: ArrayCount option
      rest: bool option }

and Mapper =
    { mappingsType: string
      mappings: Map<string, string> }










and Switch =
    { name: string option
      compareTo: string
      fields: Map<string, DataType>
      default_val: DataType option }



type Namespace =
    | Map of Map<string, Namespace>
    | DataType of DataType

type Protocol =
    { types: Map<string, DataType>
      namespaces: Map<string, Namespace> }
