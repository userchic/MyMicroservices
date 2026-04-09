import { CheckAuthorization, GatewayUrl } from "./HostName"
import { GetToken } from "./HostName"

let controllerName = "Subscriptions"
let requestBase = `${GatewayUrl}/${controllerName}/`

export async function RequestGetSubscriptions(userId: number) {
    let request = requestBase + "GetSubscriptions" + "?" + "userId=" + userId
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
export async function RequestGetSubscribers(userId: number) {
    let request = requestBase + "GetSubscribers" + "?" + "userId=" + userId
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
export async function RequestGetIsSubscribed(targetUserId: number) {
    let request = requestBase + "GetIsSubscribed" + "?" + "targetUserId=" + targetUserId
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
export async function RequestSubscribe(targetId: number) {
    let request = requestBase + "Subscribe" + "?" + "targetId=" + targetId
    let res = await fetch(request, {
        method: "Post",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res
}
export async function RequestUnsubscribe(targetId: number) {
    let request = requestBase + "Unsubscribe" + "?" + "targetId=" + targetId
    let res = await fetch(request, {
        method: "Post",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "MyAuth": `${GetToken()}`,
        },
    })
    CheckAuthorization(res)
    return await res
}

