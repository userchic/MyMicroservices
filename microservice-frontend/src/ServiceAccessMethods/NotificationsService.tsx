import { CheckAuthorization, GatewayUrl, GetToken } from "./HostName"

let controllerName = "PersonalNotifications"
let requestBase = `${GatewayUrl}/${controllerName}/`

export async function RequestGetPersonalNotificationsRule() {
    let request = requestBase + "GetPersonalNotificationsRule"
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

export async function RequestCreatePersonalNotificationsRule(NotificationService: string, InternalServiceIdentificator: string) {
    let request = requestBase + "CreatePersonalNotificationsRules"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            NotificationService: NotificationService,
            InternalServiceIdentificator: InternalServiceIdentificator
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestUpdatePersonalNotificationsRule(NotificationService: string, InternalServiceIdentificator: string) {
    let request = requestBase + "UpdatePersonalNotificationsRules"
    let res = await fetch(request, {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${GetToken()}`,
            "Content-Type": "Application/json",
            "MyAuth": `${GetToken()}`,
        },
        body: JSON.stringify({
            NotificationService: NotificationService,
            InternalServiceIdentificator: InternalServiceIdentificator
        })
    })
    CheckAuthorization(res)
    return await res.json()
}
export async function RequestDeletePersonalNotificationsRule() {
    let request = requestBase + "DeletePersonalNotificationsRules"
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