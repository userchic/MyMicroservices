import { useEffect, useState } from "react"
import { RequestCreatePost, RequestDeletePost, RequestGetUserPostsPage, RequestUpdatePost } from "../ServiceAccessMethods/PostService"
import { type Post } from "../Models/Post"
import TextArea from "antd/es/input/TextArea"
import PostInfo from "./Post"


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
            else {
                setPosts([body].concat(Posts))
                setNewPostText("")
            }

        })
    }
    function DeletePost(id: number) {
        RequestDeletePost(id).then((body) => {
            if (body.error !== undefined) {
                setMessage(body.error)
            }
            else {
                setPosts(Posts.filter((post) => post.id !== id))
            }
        })
    }
    function UpdatePost(Text: string, id: number) {
        RequestUpdatePost(id, Text).then((body) => {
            if (body.error !== undefined) {
                setMessage(body.error)
            }
            if (body.errors !== undefined) {
                setMessage(body.errors)
            }
            setPosts((currentPosts) => {
                let updatedPost = currentPosts.find((post) => post.id === id)
                updatedPost.text = body.text
                return currentPosts
            })
        })
    }
    return (
        <>
            <TextArea cols={100} rows={2} value={NewPostText} onChange={(event) => setNewPostText(event.target.value)} />
            <input type="button" onClick={CreatePost} value="Опубликовать" /><br />
            {Message}
            <h3>Посты</h3>
            {IsFirstPostsLoaded ?
                Posts.map((post) => {
                    return (
                        <>
                            <PostInfo deletePost={DeletePost} updatePost={UpdatePost} post={post} />
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