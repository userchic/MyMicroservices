import { useState } from "react"
import { RequestLogin, RequestRegister } from "../ServiceAccessMethods/UserService"
import { signIn, store } from "../Store"

export default function RegisterSection() {
    const [Login, setLogin] = useState("")
    const [Password, setPassword] = useState("")
    const [Name, setName] = useState("")
    const [Surname, setSurname] = useState("")
    const [Fatname, setFatname] = useState("")
    const [Birthday, setBirthday] = useState("")
    const [Email, setEmail] = useState("")

    const [Message, setMessage] = useState("")
    function ExecuteRegistry() {
        RequestRegister(Login, Password, Name, Surname, Fatname, Birthday !== "" ? new Date(Birthday) : new Date(), Email).then(
            (body) => {
                if (body.error !== undefined)
                    setMessage(body.error)
                else if (body.errors !== undefined) {
                    setMessage(body.errors[0])
                }
                else {
                    store.dispatch(signIn(Login))
                    setMessage("")
                    document.cookie = `Authtoken=${body.token}; Path=/;`
                    alert("Успешная регистрация в системе")
                }
            }
        )
    }
    return (
        <>
            <h3>Регистрация в системе</h3>
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
                    <tr>
                        <td>
                            Имя
                        </td>
                        <td>
                            <input type="text" value={Name} onChange={(event) => setName(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Фамилия
                        </td>
                        <td>
                            <input type="text" value={Surname} onChange={(event) => setSurname(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Отчество
                        </td>
                        <td>
                            <input type="text" value={Fatname} onChange={(event) => setFatname(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Почта
                        </td>
                        <td>
                            <input type="email" value={Email} onChange={(event) => setEmail(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            День рождения
                        </td>
                        <td>
                            <input type="date" value={Birthday} onChange={(event) => setBirthday(event.target.value)} />
                        </td>
                    </tr>
                </tbody>
            </table>
            <input type="button" value="Войти" onClick={ExecuteRegistry} />{Message}
        </>
    )
}