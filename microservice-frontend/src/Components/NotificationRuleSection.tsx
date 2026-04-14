import { useEffect, useState } from "react"
import { RequestCreatePersonalNotificationsRule, RequestDeletePersonalNotificationsRule, RequestGetPersonalNotificationsRule, RequestUpdatePersonalNotificationsRule } from "../ServiceAccessMethods/NotificationsService"
import type { NotificationRule } from "../Models/NotificationRule"

export default function NotificationsRuleSection() {
    const [NotificationRule, setNotificationRule] = useState<NotificationRule | undefined>(undefined)
    const [NotificationService, setNotificationService] = useState("mail.ru")
    const [InternalServiceIdentificator, setInternalServiceIdentificator] = useState("")
    const [RuleExists, setRuleExists] = useState(false)
    const [Message, setMessage] = useState("")
    useEffect(() => {
        RequestGetPersonalNotificationsRule().then((body) => {
            if (body.error !== undefined) {
                setMessage(body.error)
            }
            else {
                setRuleExists(true)
                setNotificationRule(body)
                setNotificationService(body.notificationService)
                setInternalServiceIdentificator(body.internalServiceIdentificator)
            }
        })
    }, [])
    function CreateRule() {
        RequestCreatePersonalNotificationsRule(NotificationService, InternalServiceIdentificator).then((body) => {
            if (body.errors !== undefined)
                setMessage(body.errors[0].errorMessage)
            else if (body.error !== undefined)
                setMessage(body.error)
            else {
                setMessage(body.message)
                setRuleExists(true)
            }
        })
    }
    function ChangeRule() {
        RequestUpdatePersonalNotificationsRule(NotificationService, InternalServiceIdentificator).then((body) => {
            if (body.errors !== undefined)
                setMessage(body.errors[0].errorMessage)
            else if (body.error !== undefined)
                setMessage(body.error)
            else {
                setMessage(body.message)

            }
        })
    }
    function DeleteRule() {
        RequestDeletePersonalNotificationsRule().then((body) => {
            if (body.error !== undefined)
                setMessage(body.error)
            else {
                setMessage(body.message)
                setRuleExists(false)

            }
        })
    }
    return (
        <>
            <h3>Установка правила уведомления</h3>
            Сервис-способ уведомления<select value={NotificationService} onChange={e => setNotificationService(e.target.value)}>
                <option value="mail.ru">mail.ru - письмо</option>
            </select><br />
            Идентификатор в рамках выбранного сервиса(Логин,ник):<input type="text" value={InternalServiceIdentificator} onChange={e => setInternalServiceIdentificator(e.target.value)} />
            <br />
            {RuleExists ?
                <>
                    <input type="button" value="Изменить правило" onClick={ChangeRule} />
                    <input type="button" value="Удалить правило" onClick={DeleteRule} />
                </>
                :
                <>
                    <input type="button" value="Создать правило" onClick={CreateRule} />

                </>
            }<br />{Message}
        </>
    )
}