module ProtodefModels

open System.Text.Json.Serialization

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
    | Byte of Signed: bool
    | Short of Signed: bool * ByteOrder: ByteOrder
    | Int of Signed: bool * ByteOrder: ByteOrder
    | Long of Signed: bool * ByteOrder: ByteOrder
    | Float of ByteOrder: ByteOrder
    | Double of ByteOrder: ByteOrder
    | VarInt of int32
    | VarLong of int64

type BitField =
    { Name: string
      Size: int
      Signed: bool }


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
    | PrefixedString of CountType: DataType
    | Loop of Loop
    | TopBitSetTerminatedArray of Structure

and Loop = { EndVal: uint32; DataType: DataType }

and Array =
    { CountType: DataType option
      Count: ArrayCount option
      ElementsType: DataType }

and Field =
    {
      [<JsonPropertyName("name")>]
      Name: string option
      [<JsonPropertyName("fieldType")>]
      FieldType: DataType
      [<JsonPropertyName("anonymous")>]      
      Anonymous: bool option }

and Count =
    {      
      CountType: DataType
      CountFor: string }



and Buffer =
    { CountType: DataType option
      Count: ArrayCount option
      Rest: bool option }

and Mapper =
    { MappingsType: string
      Mappings: Map<string, string> }










and Switch =
    { Name: string option
      CompareTo: string
      Fields: Map<string, DataType>
      Default: DataType option }



type Namespace =
    | Map of Map<string, Namespace>
    | DataType of DataType

type Protocol =
    { types: Map<string, DataType>
      namespaces: Map<string, Namespace> }
