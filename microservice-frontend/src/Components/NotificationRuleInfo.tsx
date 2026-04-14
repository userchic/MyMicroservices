import { useEffect, useState } from "react"
import type { NotificationRule } from "../Models/NotificationRule"
import { RequestGetPersonalNotificationsRule } from "../ServiceAccessMethods/NotificationsService"

export default function NotificationRuleInfo() {
    const [NotificationRule, setNotificationRule] = useState<NotificationRule | undefined>(undefined)
    const [Message, setMessage] = useState("")
    useEffect(() => {
        RequestGetPersonalNotificationsRule().then((body) => {
            if (body.error !== undefined) {
                setMessage(body.error)
            }
            else {
                setNotificationRule(body)
            }
        })
    }, [])
    return (
        <>
            {NotificationRule === undefined ?
                <>
                    Порядок уведомления:<br />
                    {Message}
                </>
                :

                <table>
                    <thead>
                        <tr>
                            <td>Порядок уведомления:</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                Сервис уведомления:
                            </td>
                            <td>
                                {NotificationRule.notificationService}
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Идентификатор внутри сервиса:
                            </td>
                            <td>
                                {NotificationRule.internalServiceIdentificator}
                            </td>
                        </tr>
                    </tbody>
                </table>
            }
        </>
    )
}