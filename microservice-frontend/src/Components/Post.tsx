import { useState } from "react"
import type { Post } from "../Models/Post"
import type { ModeType } from "antd/es/color-picker/interface"

interface Props {
    post: Post,
    deletePost: (id: number) => void
    updatePost: (Text: string, id: number) => void
}
enum Mode {
    Update,
    Read
}

export default function PostInfo({ deletePost, updatePost, post: post }: Props) {

    const [Text, setText] = useState(post.text)
    const [State, setState] = useState<Mode>(Mode.Read)
    function changeMode() {
        if (State == Mode.Read) setState(Mode.Update)
        else {
            setState(Mode.Read)
            setText(post.text)
        }
    }
    return (
        <>
            <div className="post">
                Опубликовано: {new Date(post.postTime).toLocaleDateString() + " " + new Date(post.postTime).toLocaleTimeString()}<input type="button" style={{ float: "right" }} value="X" onClick={() => deletePost(post.id)} /><input type="button" style={{ float: "right" }} value="Edit" onClick={changeMode} />
                <br />
                Сообщение:
                {
                    State == Mode.Read ? Text :
                        <>
                            <input type="text" value={Text} onChange={(event) => setText(event.target.value)} />
                            <br />
                            <input type="button" onClick={() => {
                                updatePost(Text, post.id)
                                setState(Mode.Read)
                            }} value="Подтвердить" />
                        </>
                }
            </div>
        </>
    )
}