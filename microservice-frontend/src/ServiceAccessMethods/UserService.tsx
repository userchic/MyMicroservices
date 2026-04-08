import { GatewayUrl } from "./HostName"

let controllerName = "User"
let requestBase = `${GatewayUrl}/${controllerName}/`

export async function RequestLogin(Login: string, Password: string) {
    let request = requestBase + "Login"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Login: Login, Password: Password })
    })
    let json = await res.json()
    if (json.token !== undefined)
        document.cookie = `Authtoken=${json.token}`
    return json
}
export async function RequestRegister(Login: string, Password: string, Name: string, Surname: string, Fatname: string, Birthday: Date) {
    let request = requestBase + "Register"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Login: Login,
            Password: Password,
            Name: Name,
            Surname: Surname,
            Fatname: Fatname,
            BirthDay: Birthday
        })
    })
    let json = await res.json()
    if (json.token !== undefined)
        document.cookie = `Authtoken=${json.token}`
    return json
}
export async function RequestChangeProfile(Login: string, Password: string, Name: string, Surname: string, Fatname: string, Birthday: Date) {
    let request = requestBase + "ChangeProfile"
    let res = await fetch(request, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Login: Login,
            Password: Password,
            Name: Name,
            Surname: Surname,
            Fatname: Fatname,
            BirthDay: Birthday
        })
    })
    return await res.json()

}
export async function RequestGetProfile(login: string) {
    let request = requestBase + "GetProfile" + "?" + "login=" + login
    let res = await fetch(request, {
        method: "Get",
    })
    return await res.json()
}
export async function RequestGetProfiles(login?: string, name?: string, surname?: string, fatname?: string) {
    let request = requestBase + "GetProfiles" + "?"
    if (login !== "")
        request += "login=" + login + "&"
    if (name !== "")
        request += "name=" + name + "&"
    if (surname !== "")
        request += "surname=" + surname + "&"
    if (fatname !== "")
        request += "fatname=" + fatname + "&"
    let res = await fetch(request, {
        method: "Get",
    })
    return await res.json()
}