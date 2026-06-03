import { useState } from "react"
import type { Message } from "../Models/Message"
import TextArea from "antd/es/input/TextArea"
import type { User } from "../Models/User"
import { store } from "../Store"

interface Props {
    Message: Message,
    deleteMessage: (id: number) => void
    updateMessage: (Text: string, id: number) => void,
    Owner: User
}
enum Mode {
    Read,
    Update
}
export default function Messageinfo({ Message, Owner, deleteMessage, updateMessage }: Props) {
    const [State, setState] = useState<Mode>(Mode.Read)
    const [Text, setText] = useState(Message.text)
    function changeMode() {
        if (Owner) {
            if (State == Mode.Read) setState(Mode.Update)
            else {
                setState(Mode.Read)
                setText(Message.text)
            }
        }
    }

    return (
        <>
            <div className="block" style={{ display: "flex" }}>
                <div style={{ flex: "0.8", alignContent: "center" }}>
                    {Owner?.login === store.getState().Auth.login ? null : <>{Owner.login}: <br /></>}
                    {
                        State == Mode.Read ? <div style={{ display: "inline-block" }}>{Text}</div> :
                            <>
                                <TextArea cols={100} rows={3} value={Text} onChange={(event) => setText(event.target.value)} />
                                <br />
                                <input type="button" onClick={() => {
                                    updateMessage(Text, Message.id)
                                    setState(Mode.Read)
                                }} value="Подтвердить" />
                            </>
                    }
                </div>
                <div style={{ flex: "0.2" }}>
                    {Owner?.login === store.getState().Auth.login ?
                        <>
                            <div style={{ float: "right" }}>
                                <input type="button" value="Edit" onClick={changeMode} />
                                <input type="button" value="X" onClick={() => deleteMessage(Message.id)} />
                            </div>
                        </> : null}
                    <br />
                    <div style={{ float: "right" }}>
                        Опубликовано: {new Date(Message.creationTime).toLocaleDateString() + " " + new Date(Message.creationTime).toLocaleTimeString()}
                    </div>
                </div>
                <br />
            </div>
        </>
    )
}