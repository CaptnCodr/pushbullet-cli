namespace Pushbullet    

module Patterns = 
    /// <summary>"key" | "-k"</summary> 
    let (|Key|_|) x = match x with | "key" | "-k" -> Some x | _ -> None 
    /// <summary>"me" | "-i"</summary> 
    let (|Profile|_|) x = match x with | "me" | "-i" -> Some x | _ -> None 
    /// <summary>"limits" | "-x"</summary> 
    let (|Limits|_|) x = match x with | "limits" | "-x" -> Some x | _ -> None 
    /// <summary>"grants" | "-g"</summary> 
    let (|Grants|_|) x = match x with | "grants" | "-g" -> Some x | _ -> None 
    /// <summary>"help" | "-h"</summary> 
    let (|Help|_|) x = match x with | "help" | "-h" -> Some x | _ -> None 
    /// <summary>"version" | "-v"</summary> 
    let (|Version|_|) x = match x with | "version" | "-v" -> Some x | _ -> None 
    /// <summary>"text" | "-t"</summary> 
    let (|Text|_|) x = match x with | "text" | "-t" -> Some x | _ -> None 
    /// <summary>"push" | "-p"</summary> 
    let (|Push|_|) x = match x with | "push" | "-p" -> Some x | _ -> None 
    /// <summary>"sms" | "-m"</summary>
    let (|Sms|_|) x = match x with | "sms" | "-m" -> Some x | _ -> None 
    /// <summary>"chat" | "-c"</summary> 
    let (|Chat|_|) x = match x with | "chat" | "-c" -> Some x | _ -> None 
    /// <summary>"chats" | "-cs"</summary> 
    let (|Chats|_|) x = match x with | "chats" | "-cs" -> Some x | _ -> None 
    /// <summary>"link" | "-l"</summary> 
    let (|Link|_|) x = match x with | "link" | "-l" -> Some x | _ -> None 
    /// <summary>"url" | "-u"</summary> 
    let (|Url|_|) x = match x with | "url" | "-u" -> Some x | _ -> None 
    /// <summary>"clip" | "-cl"</summary> 
    let (|Clip|_|) x = match x with | "clip" | "-cl" -> Some x | _ -> None 
    /// <summary>"pushes" | "-ps"</summary> 
    let (|Pushes|_|) x = match x with | "pushes" | "-ps" -> Some x | _ -> None  
    /// <summary>"pushinfo" | "-pi"</summary> 
    let (|PushInfo|_|) x = match x with | "pushinfo" | "-pi" -> Some x | _ -> None 
    /// <summary>"delete" | "-del" | "remove" | "-r"</summary> 
    let (|Delete|_|) x = match x with | "delete" | "-del" | "remove" | "-r" -> Some x | _ -> None 
    /// <summary>"device" | "-d"</summary> 
    let (|Device|_|) x = match x with | "device" | "-d" -> Some x | _ -> None 
    /// <summary>"devices" | "-ds"</summary> 
    let (|Devices|_|) x = match x with | "devices" | "-ds" -> Some x | _ -> None 
    /// <summary>"subscription" | "-s"</summary> 
    let (|Subscription|_|) x = match x with | "subscription" | "-s" -> Some x | _ -> None 
    /// <summary>"subscriptions" | "subs" | "-s"</summary> 
    let (|Subscriptions|_|) x = match x with | "subscriptions" | "subs" | "-s" -> Some x | _ -> None 
    /// <summary>"channelinfo" | "ci"</summary> 
    let (|ChannelInfo|_|) x = match x with | "channelinfo" | "ci" -> Some x | _ -> None 