import { useState } from "react"
import { RequestLogin } from "../ServiceAccessMethods/UserService"
import { signIn, store } from "../Store"

export default function LoginSection() {
    const [Login, setLogin] = useState("")
    const [Password, setPassword] = useState("")
    const [Message, setMessage] = useState("")
    function ExecuteLogin() {
        RequestLogin(Login, Password).then(
            (body) => {
                if (body.error !== undefined)
                    setMessage(body.error)
                else if (body.errors !== undefined) {
                    setMessage(body.errors[0])
                }
                else {
                    store.dispatch(signIn(Login))
                    setMessage("")
                    alert("Успешный вход в систему")
                }
            }
        )
    }
    return (
        <>
            <h3>Вход в систему</h3>
            <table>
                <tbody>
                    <tr>
                        <td>
                            Логин
                        </td>
                        <td>
                            <input type="text" value={Login} onChange={(event) => setLogin(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Пароль
                        </td>
                        <td>
                            <input type="password" value={Password} onChange={(event) => setPassword(event.target.value)} />
                        </td>
                    </tr>
                </tbody>
            </table>
            <input type="button" value="Войти" onClick={ExecuteLogin} />{Message}
        </>
    )
}