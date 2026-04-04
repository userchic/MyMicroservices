import { GatewayUrl } from "./HostName"

let controllerName = "TextPost"
let requestBase = `${GatewayUrl}/${controllerName}/`
function GetToken(): string | undefined {
    let tokenCookie = document.cookie.split(" ").find((cookie) => {
        return cookie.startsWith("Authtoken=")
    })?.slice(10)
    return tokenCookie
}
export async function RequestCreatePost(Text: string) {
    let request = requestBase + "CreatePost"
    let res = await fetch(request, {
        credentials: "include",
        method: "POST",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Text: Text })
    })
    return await res.json()
}
export async function RequestDeletePost(id: number) {
    let request = requestBase + "DeletePost" + "?" + "id=" + id
    let res = await fetch(request, {
        credentials: "include",
        method: "POST",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Text: Text })
    })
    return await res.json()
}
export async function RequestUpdatePost(Id: number, Text: string) {
    let request = requestBase + "UpdatePost"
    let res = await fetch(request, {
        credentials: "include",
        method: "Put",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Text: Text, Id: Id })
    })
    return await res.json()
}
export async function RequestGetPost(id: number) {
    let request = requestBase + "GetPost" + "?" + "id=" + id
    let res = await fetch(request, {
        credentials: "include",
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`
        }
    })
    return await res.json()
}
export async function RequestGetUserPostsPage(page: number, userId: number) {
    let request = requestBase + "GetUserPostsPage" + "?" + "page=" + page + "&" + "userId=" + userId
    let res = await fetch(request, {
        method: "Get",
        credentials: "include",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
        }
    })
    return await res.json()
}