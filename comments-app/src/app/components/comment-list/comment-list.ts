import { Component, Input, OnInit } from '@angular/core';
import { CommentRead, CommentListResponse } from '../../models/comment.model';
import { CommentService } from '../../services/comment.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-comment-list',
  imports: [CommonModule],
  templateUrl: './comment-list.html',
  styleUrls: ['./comment-list.css'],
  standalone: true,
})
export class CommentList implements OnInit {
  @Input() comments: CommentRead[] = [];

replyPaginationMap: {
  [parentId: number]: {
    page: number;
    pageSize: number;
    totalPages: number;
  }
} = {};

  // Картка з дочірніми коментарями (id батька => replies)
  commentRepliesMap: { [parentId: number]: CommentRead[] } = {};

  // Множина id коментарів, які розгорнуті для показу відповідей
  expandedComments = new Set<number>();

  // Параметри сортування (інтегровано)
  sortBy: string = 'createdAt';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Параметри пагінації
  pageSize = 5;
  currentPage = 1;
  totalCount = 0;

  constructor(private commentService: CommentService) {}

  ngOnInit(): void {
    if(this.comments?.length == 0)
      this.loadComments();
  }

  // Завантаження коментарів із сервера з урахуванням пагінації та сортування
  loadComments(): void {
    this.commentService.getComments(
      this.sortBy,
      this.sortDirection,
      this.currentPage,
      this.pageSize,
    ).subscribe({
      next: (data: CommentListResponse) => {
        this.comments = data.items;
        this.totalCount = data.totalCount;
        this.expandedComments.clear();
        this.commentRepliesMap = {};
      },
      error: (err) => {
        console.error('Failed to load comments', err);
      }
    });
  }

  // Показати / приховати відповіді
  toggleReplies(comment: CommentRead): void {
    if (this.isExpanded(comment)) {
      this.expandedComments.delete(comment.id);
    } else {
      this.expandedComments.add(comment.id);
      if (!this.commentRepliesMap[comment.id]) {
        this.loadReplies(comment);
      }
    }
  }

  // Перевірка чи розгорнутий коментар
  isExpanded(comment: CommentRead): boolean {
    return this.expandedComments.has(comment.id);
  }

loadReplies(comment: CommentRead, page: number = 1): void {
  this.commentService.getReplies(comment.id, this.sortBy, this.sortDirection, 1, 100).subscribe({
  next: (response) => {
    this.commentRepliesMap[comment.id] = response.items;

    this.replyPaginationMap[comment.id] = {
      page: page,
      pageSize: 100,
      totalPages: 1,
    };
  },
    error: (err) => {
      console.error('Failed to load replies', err);
      this.commentRepliesMap[comment.id] = [];
    }
  });
}

changeReplyPage(commentId: number, newPage: number) {
  this.loadReplies({ id: commentId } as CommentRead, newPage);
}

  // Сортування таблиці
  sort(field: keyof CommentRead): void {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortDirection = 'asc';
    }
  }

  // Переходи між сторінками пагінації
  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadComments();
  }

  // Загальна кількість сторінок
  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
