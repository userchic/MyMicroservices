import { useEffect, useState } from "react"
import { store } from "../Store"
import { RequestGetDialogsPage } from "../ServiceAccessMethods/MessageService"
import type { Dialog } from "../Models/Dialog"
import DialogInfo from "./DialogInfo"

export default function DialogSection() {
    const [Dialogs, setDialogs] = useState<Dialog[]>([])
    const [Page, setPage] = useState(1)
    useEffect(() => {
        RequestGetDialogsPage(Page).then((body) => {
            setDialogs([...Dialogs, ...body])
        })
        setPage(Page + 1)
    }, [])
    return (
        <>
            <h3>Диалоги</h3>
            {Dialogs?.map((dialog) => {
                return (
                    <DialogInfo Dialog={dialog} />
                )
            })}
        </>
    )
}