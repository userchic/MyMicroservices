import { useState } from "react"
import type { Post } from "../Models/Post"
import type { ModeType } from "antd/es/color-picker/interface"
import TextArea from "antd/es/input/TextArea"

interface Props {
    post: Post,
    deletePost: (id: number) => void
    updatePost: (Text: string, id: number) => void,
    isOwned: boolean
}
enum Mode {
    Update,
    Read
}

export default function PostInfo({ deletePost, updatePost, post: post, isOwned }: Props) {

    const [Text, setText] = useState(post.text)
    const [State, setState] = useState<Mode>(Mode.Read)
    function changeMode() {
        if (isOwned) {
            if (State == Mode.Read) setState(Mode.Update)
            else {
                setState(Mode.Read)
                setText(post.text)
            }
        }
    }
    return (
        <>
            <div style={{ display: "flex" }}>
                <div style={{ flex: "0.8", alignContent: "center" }}>
                    {
                        State == Mode.Read ? <div style={{ display: "inline-block" }}>{Text}</div> :
                            <>
                                <TextArea cols={100} rows={3} value={Text} onChange={(event) => setText(event.target.value)} />
                                <br />
                                <input type="button" onClick={() => {
                                    updatePost(Text, post.id)
                                    setState(Mode.Read)
                                }} value="Подтвердить" />
                            </>
                    }
                </div>
                <div style={{ flex: "0.2" }}>
                    {isOwned ?
                        <>
                            <div style={{ float: "right" }}>
                                <input type="button" value="Edit" onClick={changeMode} />
                                <input type="button" value="X" onClick={() => deletePost(post.id)} />
                            </div>
                        </> : null}
                    <br />
                    <div style={{ float: "right" }}>
                        Опубликовано: {new Date(post.postTime).toLocaleDateString() + " " + new Date(post.postTime).toLocaleTimeString()}
                    </div>

                </div>
            </div>
        </>
    )
}