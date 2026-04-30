import { useEffect, useState } from "react"
import { RequestCreateComment, RequestGetCommentsPage } from "../ServiceAccessMethods/CommentsService"
import CommentInfo from "./CommentInfo"
import type { Comment } from "../Models/Comment"
import InputEmoji from "react-input-emoji"

interface Props {
    postId: number,

}
export default function CommentSection({ postId }: Props) {
    const [Comments, setComments] = useState<Comment[]>([])
    const [Page, setPage] = useState(1)
    const [Text, setText] = useState("")
    const [isOutOfPosts, setIsOutOfPosts] = useState(false)
    useEffect(() => {
        GetCommentsPage();
    }, [])
    function CreateComment() {
        RequestCreateComment(Text, postId).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else if (body.errors !== undefined) {
                alert(body.errors[0])
            }
            else {
                setComments([...body, Comments])
                setText("")
            }
        })
    }
    function GetCommentsPage() {
        RequestGetCommentsPage(postId, 1).then((body) => {
            if (body.error !== undefined) {
                alert(body.error)
            }
            else {
                let newCommentList = Comments.concat(body)
                setComments(newCommentList)
                setPage(Page + 1)
                setIsOutOfPosts(body.length == 0)
            }
        })
    }
    function DeleteComment(commentId: number) {
        setComments(Comments?.filter((comment) => {
            return comment.id !== commentId
        }))
    }
    return (
        <>
            <div className="block">
                <InputEmoji value={Text} onChange={(string) => setText(string)} shouldReturn={true} shouldConvertEmojiToImage={false} />
                <input type="button" value="Отправить" onClick={CreateComment} />
            </div>
            {Comments?.map((comment) => {
                return (
                    <>
                        <div className="block">
                            <CommentInfo comment={comment} Delete={(id) => DeleteComment(id)} />
                        </div>
                    </>)
            })}
        </>
    )
}