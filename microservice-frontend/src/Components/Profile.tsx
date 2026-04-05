import type { User } from "../Models/User";

interface Props {
    User: User
}
export default function Profile({ User }: Props) {
    return (
        <>
            <table>
                <tbody>
                    <tr>
                        <td>
                            ФИО:
                        </td>
                        <td>
                            {User?.name} {User?.surname} {User?.fatname}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            День рождения:
                        </td>
                        <td>
                            {new Date(User?.birthday).toLocaleDateString()}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Логин:
                        </td>
                        <td>
                            {User?.login}
                        </td>
                    </tr>
                </tbody>
            </table>
        </>
    )
}