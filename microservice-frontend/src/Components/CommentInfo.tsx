import { useEffect, useState } from "react"
import { type User } from "../Models/User";
import type { Comment } from "../Models/Comment";
import InputEmoji from "react-input-emoji"
import { RequestGetProfile, RequestGetProfileById } from "../ServiceAccessMethods/UserService";
import { RequestDeleteComment, RequestUpdateComment } from "../ServiceAccessMethods/CommentsService";
import { store } from "../Store";
enum Mode {
    Update,
    Read
}
interface Props {
    comment: Comment,
    Delete: (id: number) => void
}

export default function CommentInfo({ Delete, comment }: Props) {
    const [User, setUser] = useState<User | undefined>(undefined)
    const [isOwned, setIsOwned] = useState<boolean>(false)
    const [Text, setText] = useState(comment.text)
    const [State, setState] = useState<Mode>(Mode.Read)
    useEffect(() => {
        RequestGetProfileById(comment.ownerUserId).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else {
                setUser(body)
                setIsOwned(body.login === store.getState().Auth.login)
            }
        })
    })
    function changeMode() {
        if (isOwned) {
            if (State == Mode.Read) setState(Mode.Update)
            else {
                setState(Mode.Read)
            }
        }
    }
    function DeleteComment() {
        RequestDeleteComment(comment.id).then((body) => {
            if (body.error !== undefined)
                alert(body.error)
            else
                Delete(comment.id)
        })
    }
    function UpdateComment() {
        RequestUpdateComment(Text, comment.id).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else if (body.errors !== undefined) {
                alert(body.errors[0])
            }
            else {
                setState(Mode.Read)

            }

        })
    }
    return (
        <>
            {User !== undefined ?
                <>
                    {User.name} {User.surname} {User.fatname}
                </>
                :
                null
            }<br />

            <div className="no-margin-block" style={{ display: "flex" }}>
                <div style={{ flex: "0.8", alignContent: "center" }}>
                    {
                        State == Mode.Read ? <div style={{ display: "inline-block" }}>{Text}</div> :
                            <>
                                <InputEmoji value={Text} onChange={(text) => setText(text)} shouldReturn={true} shouldConvertEmojiToImage={false} />
                                <br />
                                <input type="button" onClick={() => {
                                    UpdateComment()
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
                                <input type="button" value="X" onClick={() => DeleteComment()} />
                            </div>
                        </> : null}
                    <br />
                    <div style={{ float: "right" }}>
                        Опубликовано: {new Date(comment.creationTime).toLocaleDateString() + " " + new Date(comment.creationTime).toLocaleTimeString()}
                    </div>

                </div>
            </div>
        </>
    )
}