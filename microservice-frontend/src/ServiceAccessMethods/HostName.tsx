import { signOut, store } from "../Store"

export let GatewayUrl = "http://localhost:8080"

export function GetToken(): string | undefined {
    let tokenCookie = document.cookie.split(" ").find((cookie) => {
        return cookie.startsWith("Authtoken=")
    })?.slice(10)
    return tokenCookie
}
export function CheckAuthorization(response: Response) {
    if (response.status == 401 && store.getState().Auth.login !== "") {
        store.dispatch(signOut())
        alert("Срок аутентификации истек")
    }
}