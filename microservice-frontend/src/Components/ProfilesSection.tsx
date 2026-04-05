import { useEffect, useState } from "react";
import type { User } from "../Models/User";
import { RequestGetFiltered } from "../ServiceAccessMethods/UserService";
import Profile from "./Profile";
import { Link } from "react-router";

export default function ProfilesSection() {
    const [Login, setLogin] = useState("")
    const [Name, setName] = useState("")
    const [Surname, setSurname] = useState("")
    const [Fatname, setFatname] = useState("")

    const [Message, setMessage] = useState("")

    const [Profiles, setProfiles] = useState<User[]>([])
    useEffect(() => Search())
    function Search() {
        RequestGetFiltered(Login, Name, Surname, Fatname).then((body) => {
            setProfiles(body)
            if (body.length == 0)
                setMessage("Нет таких профилей ")
            else
                setMessage("")
        })
    }
    return (
        <>
            <h2>Поиск</h2>
            <table>
                <tbody>
                    <tr>
                        <td>
                            Логин:
                        </td>
                        <td>
                            <input type="text" value={Login} onChange={(event) => setLogin(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Имя:
                        </td>
                        <td>
                            <input type="text" value={Name} onChange={(event) => setName(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Фамилия:
                        </td>
                        <td>
                            <input type="text" value={Surname} onChange={(event) => setSurname(event.target.value)} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Отчество:
                        </td>
                        <td>
                            <input type="text" value={Fatname} onChange={(event) => setFatname(event.target.value)} />
                        </td>
                    </tr>
                </tbody>
            </table>
            <input type="button" value="Найти" onClick={Search} />
            {Message}

            {Profiles.map((profile) => {
                return (
                    <>
                        <div className="block">
                            <Profile User={profile} />
                            <Link to={`/Profile/${profile.login}`} > Перейти к профилю</Link>
                        </div>
                    </>
                )
            }
            )}
        </>
    )
}