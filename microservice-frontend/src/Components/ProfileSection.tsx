import { useParams } from "react-router";
import { store } from "../Store";
import { useEffect, useState } from "react";
import type { User } from "../Models/User";
import { RequestGetProfile } from "../ServiceAccessMethods/UserService";
import { RequestGetUserPostsPage } from "../ServiceAccessMethods/PostService";
import PostsSection from "./PostsSection";

export default function ProfileSection() {
    const params = useParams()
    const userLogin = params.id !== undefined ? params.id : store.getState().Auth.login
    const [IsProfileOwner] = useState(userLogin === store.getState().Auth.login)
    const [User, setUser] = useState<User | undefined>(undefined)
    const [IsProfileLoaded, setIsProfileLoaded] = useState(false)
    const [Message, setMessage] = useState("")
    useEffect(() => {
        RequestGetProfile(userLogin).then((body) => {
            if (body.error !== undefined)
                setMessage(body.error)
            else {
                setUser(body)
                setIsProfileLoaded(true)
            }
        })
    }, [])
    return (
        <>
            <h2>Профиль</h2>
            {IsProfileLoaded ?
                <>
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    ФИО:
                                </td>
                                <td>
                                    {User?.name} {User?.surname} {User?.fatname}
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    День рождения:
                                </td>
                                <td>
                                    {new Date(User?.birthday).toLocaleDateString()}
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Логин:
                                </td>
                                <td>
                                    {User?.login}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <PostsSection IsProfileOwner={IsProfileOwner} ProfileId={User.id} />
                </>
                : "Профиль загружается"}
            {Message}
        </>
    )
}