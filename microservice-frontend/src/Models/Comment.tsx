export interface Comment {
    id: number,
    ownerUserId: number,
    postId: number,
    text: string,
    creationTime: Date
}