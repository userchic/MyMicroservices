import { useEffect, useState } from "react"
import { RequestCreatePost, RequestGetUserPostsPage } from "../ServiceAccessMethods/PostService"
import { type Post } from "../Models/Post"
import TextArea from "antd/es/input/TextArea"

interface Props {
    ProfileId: number,
    IsProfileOwner: boolean
}
export default function PostsSection({ ProfileId, IsProfileOwner }: Props) {
    const [Page, setPage] = useState(1)
    const [Posts, setPosts] = useState<Post[]>([])
    const [NewPostText, setNewPostText] = useState("")
    const [Message, setMessage] = useState("")
    const [IsFirstPostsLoaded, setIsFirstPostsLoaded] = useState(false)
    const [IsOutOfPosts, setIsOutOfPosts] = useState(false)
    useEffect(() => {
        LoadPosts()
        setIsFirstPostsLoaded(true)
    }, [])
    function LoadPosts() {
        RequestGetUserPostsPage(Page, ProfileId).then((body => {
            if (body.error !== undefined) {
                setMessage(body.error)
            }
            else {

                setPosts(Posts.concat(body))
                setPage(Page + 1)
                if (body.length == 0)
                    setIsOutOfPosts(true)
            }
        }))
    }
    function CreatePost() {
        RequestCreatePost(NewPostText).then((body) => {
            if (body.error !== undefined)
                setMessage(body.error)
            else if (body.errors !== undefined)
                setMessage(body.errors[0])
            else
                setPosts(body.concat(Posts))

        })
    }
    return (
        <>
            <TextArea cols={100} rows={3} value={NewPostText} onChange={(event) => setNewPostText(event.target.value)} />
            <input type="button" onClick={CreatePost} value="Опубликовать" /><br />
            {Message}
            <h3>Посты</h3>
            {IsFirstPostsLoaded ?
                Posts.map((post) => {
                    return (
                        <>
                            <div style={{ padding: "5px" }}>
                                Опубликовано: {post.PostTime.toLocaleString()}
                                Сообщение:
                                {post.Text}
                            </div>
                        </>
                    )
                })
                : "Посты загружаются"
            }
            {
                IsOutOfPosts ? null : <input type="button" onClick={LoadPosts} value="Еще" />
            }
        </>
    )
}