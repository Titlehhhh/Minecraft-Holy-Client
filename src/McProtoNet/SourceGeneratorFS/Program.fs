module Program

open System.Collections.Generic
open System.Diagnostics
open System.Runtime.InteropServices.JavaScript
open Microsoft.FSharp.Control
open SourceGenerator
open SourceGenerator.ProtoDefTypes
open System




[<Literal>]
let root = "data"

let McProtoNetPath =
    "C:\\Users\\Title\\source\\repos\\Minecraft-Holy-Client\\src\\McProtoNet\\McProtoNet.Protocol"

let collection =
    MinecraftData.ReadAllAsync(root) |> Async.AwaitTask |> Async.RunSynchronously

let cloneCollection = collection.Clone()

let protocols =
    cloneCollection.Protocols |> Seq.map (_.Value.JsonPackets) |> Seq.toArray


let first = Array.head protocols

let firstPlayClientPackets = first["play.toServer"]

let supportedTypes = [| "position"; "slot" |]




let processPassContainers (first: ProtodefContainer, second: ProtodefContainer) =
    let g = 10
    ()

let filterCustom (field: ProtodefContainerField) : bool =
    if field.IsAnon then
        false
    else if field.Type :? ProtodefCustomType then
        let cus: ProtodefCustomType = field.Type :?> ProtodefCustomType
        Array.contains cus.Name supportedTypes
    else
        false

let skippedPackets = HashSet<string>()

let processPass (first: ProtodefNamespace, second: ProtodefNamespace) =
    let KeysBefore = first.Types.Keys
                          |> Seq.append second.Types.Keys
                          |> Seq.filter (fun x-> x <> "packet")
                          |> Set.ofSeq
    
    first.FilterSimple filterCustom
    second.FilterSimple filterCustom


    let keys1 = first.Types.Keys |> Seq.except second.Types.Keys |> Seq.toArray

    let keys2 = second.Types.Keys |> Seq.except first.Types.Keys |> Seq.toArray

    let keys = keys1 |> Seq.append keys2 |> Seq.toArray

    for key in keys do        
        first.Types.Remove key |> ignore
        second.Types.Remove key |> ignore

    
    if first.Types.Count <> second.Types.Count then
        raise (Exception("count not equal"))
    
    let keysAfter = first.Types.Keys
                          |> Seq.append second.Types.Keys
                          |> Seq.filter (fun x-> x <> "packet")
                          |> Set.ofSeq
    let deleted = KeysBefore |> Seq.except keysAfter
    
    for s in deleted do
        skippedPackets.Add(s)
    ()
    
    (*
    let a =
        first
        |> Seq.zip second
        |> Seq.map (fun x -> KeyValuePair((fst x).Key, ((fst x).Value, (snd x).Value)))

    for KeyValue(name, typesTuple) in a do
        processPassContainers (fst typesTuple :?> ProtodefContainer, snd typesTuple :?> ProtodefContainer) *)




for play2S in protocols |> Seq.skip 1 do
    
    processPass (firstPlayClientPackets, play2S["play.toServer"])

for p in skippedPackets do
    p |> printfn "%s"