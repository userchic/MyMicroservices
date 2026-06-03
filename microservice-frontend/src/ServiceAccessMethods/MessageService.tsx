import { CheckAuthorization, GatewayUrl, GetToken } from "./HostName"

let controllerName = "Message"
let requestBase = `${GatewayUrl}/${controllerName}/`

export async function RequestGetDialogsPage(page: number) {
    let request = requestBase + "GetDialogsPage" + "?" + "page=" + page
    let res = await fetch(request, {
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestGetDialog(targetUserId: number) {
    let request = requestBase + "GetDialog" + "?" + "targetUserId=" + targetUserId
    let res = await fetch(request, {
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestGetMessagesPageFromDialog(page: number, dialogId: number) {
    let request = requestBase + "GetMessagesPageFromDialog" + "?" + "page=" + page + "&dialogId=" + dialogId
    let res = await fetch(request, {
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestSendMessage(receiverId: number, text: string) {
    let request = requestBase + "SendMessage"
    let res = await fetch(request, {
        method: "Post",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            RecieverId: receiverId,
            Text: text
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestUpdateMessage(messageId: number, newText: string) {
    let request = requestBase + "UpdateMessage"
    let res = await fetch(request, {
        method: "Put",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            MessageId: messageId,
            NewText: newText
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestDeleteMessage(messageId: number) {
    let request = requestBase + "DeleteMessage" + "?" + "messageID=" + messageId
    let res = await fetch(request, {
        method: "Delete",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res.json()
}