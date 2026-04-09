
import { CheckAuthorization, GatewayUrl, GetToken } from "./HostName"

let controllerName = "TextPost"
let requestBase = `${GatewayUrl}/${controllerName}/`


export async function RequestCreatePost(text: string) {
    let request = requestBase + "CreatePost"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({ Text: text })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestDeletePost(id: number) {
    let request = requestBase + "DeletePost" + "?" + "id=" + id
    let res = await fetch(request, {
        method: "Delete",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({ Text: Text })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestUpdatePost(id: number, text: string) {
    let request = requestBase + "UpdatePost"
    let res = await fetch(request, {
        method: "Put",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({ Text: text, Id: id })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestGetPost(id: number) {
    let request = requestBase + "GetPost" + "?" + "id=" + id
    let res = await fetch(request, {
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        }
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestGetUserPostsPage(page: number, userId: number) {
    let request = requestBase + "GetUserPostsPage" + "?" + "page=" + page + "&" + "userId=" + userId
    let res = await fetch(request, {
        method: "Get",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        }
    })
    CheckAuthorization(res)
    return await res.json()
}
