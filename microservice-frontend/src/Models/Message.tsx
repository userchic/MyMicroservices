import type { Dialog } from "./Dialog";

export interface Message {
    id: number,
    dialogId: number,
    senderId: number,
    text: string,
    creationTime: Date,
    //dialog: Dialog
}