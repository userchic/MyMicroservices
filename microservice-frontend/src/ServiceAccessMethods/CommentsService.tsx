import { CheckAuthorization, GatewayUrl, GetToken } from "./HostName"

let controllerName = "Comments"
let requestBase = `${GatewayUrl}/${controllerName}/`

export async function RequestGetCommentsPage(postId: number, page: number) {
    let request = requestBase + "GetCommentsPage" + "?" + "postId=" + postId + "&" + "page=" + page
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


export async function RequestCreateComment(text: string, postId: number) {
    let request = requestBase + "CreateComment"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            Text: text,
            PostId: postId
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestUpdateComment(newText: string, commentId: number) {
    let request = requestBase + "UpdateComment"
    let res = await fetch(request, {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            NewText: newText,
            CommentId: commentId
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestDeleteComment(commentId: number) {
    let request = requestBase + "DeleteComment" + "?" + "commentId=" + commentId
    let res = await fetch(request, {
        method: "DELETE",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res.json()
}