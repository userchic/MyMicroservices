import { useEffect, useState } from "react"
import type { Message } from "../Models/Message"
import type { Dialog } from "../Models/Dialog"
import type { User } from "../Models/User"
import { RequestDeleteMessage, RequestGetDialog, RequestGetDialogsPage, RequestGetMessagesPageFromDialog, RequestSendMessage, RequestUpdateMessage } from "../ServiceAccessMethods/MessageService"
import { RequestGetCommentsPage } from "../ServiceAccessMethods/CommentsService"
import { RequestGetProfile, RequestGetProfileById } from "../ServiceAccessMethods/UserService"
import { store } from "../Store"
import Messageinfo from "./MessageInfo"
import { useParams } from "react-router"
import InputEmoji from "react-input-emoji"

export default function MessageSection() {
    const params = useParams()
    const targetUserId = parseInt(params.targetUserId)
    const [Text, setText] = useState("")
    const [Dialog, setDialog] = useState<Dialog | null>(null)
    const [Messages, setMessages] = useState<Message[]>([])
    const [OperatingUser, setOperatingUser] = useState<User | null>(null)
    const [OpponentUser, setOpponentUser] = useState<User | null>(null)
    const [Page, setPage] = useState(1)

    useEffect(() => {
        RequestGetDialog(targetUserId).then((body: Dialog) => {
            if (body !== undefined && body !== null) {
                setDialog(body)
                RequestGetProfileById(body.user1Id).then((body: User) => {
                    if (body.login === store.getState().Auth.login)
                        setOperatingUser(body)
                    else
                        setOpponentUser(body)
                })
                RequestGetProfileById(body.user2Id).then((body: User) => {
                    if (body.login === store.getState().Auth.login)
                        setOperatingUser(body)
                    else
                        setOpponentUser(body)
                })
                getMessages(body.id)
            }
            else
                RequestGetProfileById(targetUserId).then((body: User) => {
                    setOpponentUser(body)
                })
        })

    }, [])
    function getMessages(dialogId: number) {
        RequestGetMessagesPageFromDialog(Page, dialogId).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else {
                setMessages(body)
            }
        })
    }
    function createMessage() {
        RequestSendMessage(targetUserId, Text).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else if (body.errors !== undefined) {
                alert(body.errors[0].errorMessage)
            }
            else {
                setMessages([body, ...Messages])
                setText("")
            }
        })
    }
    function updateMessage(text: string, id: number): void {
        RequestUpdateMessage(id, text).then((body) => {
            if (body.errors !== undefined)
                alert(body.errors[0].errorMessage)
            else if (body.error !== undefined)
                alert(body.error)
            setMessages((currentMessages) => {
                let updatedMessage = currentMessages.find((message) => message.id === id)
                updatedMessage.text = body.text
                return currentMessages
            })
        })
    }

    function deleteMessage(id: number): void {
        RequestDeleteMessage(id).then((body) => {
            if (body.errors !== undefined)
                alert(body.errors[0].errorMessage)
            else if (body.error !== undefined)
                alert(body.error)
            else {
                setMessages(Messages.filter((message) => message.id !== id))
            }
        })
    }
    return (
        <>
            <div>
                <div className="block">
                    <h3>Диалог</h3>
                    Собеседник: {OpponentUser?.login} {OpponentUser?.name} {OpponentUser?.surname} {OpponentUser?.fatname}
                    <InputEmoji value={Text} onChange={(string) => setText(string)} shouldReturn={true} shouldConvertEmojiToImage={false} />
                    <input type="button" value="Отправить" onClick={createMessage} />
                    {Messages?.map((message) => {
                        return (
                            <>
                                <Messageinfo Message={message} Owner={message.senderId == OperatingUser?.id ? OperatingUser : OpponentUser} deleteMessage={deleteMessage} updateMessage={updateMessage} key={message.id} />
                            </>
                        )
                    }
                    )}
                </div>
            </div>
        </>
    )
}