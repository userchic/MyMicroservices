import type { Message } from "./Message";

export interface Dialog {
    id: number,
    user1Id: number,
    user2Id: number,
    messages: Message[]
}