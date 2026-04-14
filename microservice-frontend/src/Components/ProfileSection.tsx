import { useParams } from "react-router";
import { store } from "../Store";
import { useEffect, useState } from "react";
import type { User } from "../Models/User";
import { RequestGetProfile } from "../ServiceAccessMethods/UserService";
import PostsSection from "./PostsSection";
import Profile from "./Profile";
import NotificationRuleInfo from "./NotificationRuleInfo";

export default function ProfileSection() {
    const params = useParams()
    const userLogin = params.login !== undefined ? params.login : store.getState().Auth.login
    const [IsProfileOwner] = useState(userLogin === store.getState().Auth.login)
    const [User, setUser] = useState<User | undefined>(undefined)
    const [IsProfileLoaded, setIsProfileLoaded] = useState(false)
    const [Message, setMessage] = useState("")
    useEffect(() => {
        RequestGetProfile(userLogin).then((body) => {
            if (body.error !== undefined)
                setMessage(body.error)
            else {
                setUser(body)
                setIsProfileLoaded(true)
            }
        })
    }, [])
    return (
        <>
            <h2>Профиль</h2>
            {IsProfileLoaded ?
                <>
                    <div className="block">
                        <div style={{ display: "flex" }}>
                            <div style={{ flex: "0.5" }}>
                                <Profile User={User} IsProfileOwner={IsProfileOwner} />
                            </div>
                            <div style={{ flex: "0.5" }}>
                                <NotificationRuleInfo />
                            </div>
                        </div>
                    </div>
                    <PostsSection IsProfileOwner={IsProfileOwner} ProfileId={User.id} />
                </>
                : "Профиль загружается"}
            {Message}
        </>
    )
}