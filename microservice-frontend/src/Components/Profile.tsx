import { useEffect, useState } from "react";
import type { User } from "../Models/User";
import { RequestGetIsSubscribed, RequestSubscribe, RequestUnsubscribe } from "../ServiceAccessMethods/SubscribtionService";
import { store } from "../Store";

interface Props {
    User: User,
}
export default function Profile({ User }: Props) {
    const [IsProfileOwner] = useState(store.getState().Auth.login === User.login)
    const [IsSubscribed, setIsSubscribed] = useState<undefined | boolean>(undefined)
    useEffect(() => {
        if (!IsProfileOwner) {
            RequestGetIsSubscribed(User.id).then((body) => {
                setIsSubscribed(body.isSubscribed === "True")
            })
        }
    }, [])
    async function Subscribe() {
        RequestSubscribe(User.id).then((request) => {
            if (request.ok) {
                setIsSubscribed(true)
            }
        })
    }
    async function Unsubscribe() {
        RequestUnsubscribe(User.id).then((request) => {
            if (request.ok) {
                setIsSubscribed(false)
            }
        })
    }
    return (
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
            {
                IsSubscribed !== undefined ?
                    IsSubscribed !== false ?
                        <input type="button" value="Отписаться" onClick={Unsubscribe} />
                        :
                        <input type="button" value="Подписаться" onClick={Subscribe} />
                    :
                    <strong>Это вы</strong>
            }
        </>
    )
}