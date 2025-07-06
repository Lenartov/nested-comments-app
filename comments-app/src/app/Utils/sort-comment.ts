import { CommentRead } from '../models/comment.model';

export function sortComments<T extends keyof CommentRead>(
  comments: CommentRead[],
  field: T,
  direction: 'asc' | 'desc'
): CommentRead[] {
  return [...comments].sort((a, b) => {
    const valA = a[field];
    const valB = b[field];

    const compareA = field === 'createdAt' && typeof valA === 'string' ? new Date(valA) : valA;
    const compareB = field === 'createdAt' && typeof valB === 'string' ? new Date(valB) : valB;

    if (compareA == null) return 1;
    if (compareB == null) return -1;

    if (compareA < compareB) return direction === 'asc' ? -1 : 1;
    if (compareA > compareB) return direction === 'asc' ? 1 : -1;
    return 0;
  });
}
