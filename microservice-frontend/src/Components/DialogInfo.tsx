import { useNavigate } from "react-router";
import type { Dialog } from "../Models/Dialog";
import { store } from "../Store";
import { useEffect, useState } from "react";
import type { User } from "../Models/User";
import { RequestGetProfile, RequestGetProfileById } from "../ServiceAccessMethods/UserService";

interface Props {
    Dialog: Dialog
}
export default function DialogInfo({ Dialog }: Props) {
    const navigate = useNavigate()
    const [OpponentUser, setOpponentUser] = useState<User | null>(null)
    function openDialog() {
        navigate(`/Messages/${OpponentUser?.id}`)
    }
    useEffect(() => {
        RequestGetProfileById(Dialog.user1Id).then((body: User) => {
            if (body.login !== store.getState().Auth.login)
                setOpponentUser(body)
        }).then(() =>
            RequestGetProfileById(Dialog.user2Id).then((body: User) => {
                if (body.login !== store.getState().Auth.login)
                    setOpponentUser(body)
                else
                    if (body === null)
                        setOpponentUser({ birthday: new Date(), email: "", fatname: "", id: 1, login: "Это вы", name: "", password: "", surname: "" })
            })
        )

    }, [])
    return (
        <>
            <div className="block" style={{ display: "flex" }}>
                <div style={{ flex: "0.8", alignContent: "center" }}>
                    Логин:{OpponentUser?.login}
                </div>
                <div style={{ flex: "0.2" }}>
                    <div style={{ float: "right" }}>
                        <input type="button" style={{ height: "50px", borderRadius: "10px" }} onClick={openDialog} value="Открыть диалог" />
                    </div>
                </div>
            </div>
        </>
    )
}

