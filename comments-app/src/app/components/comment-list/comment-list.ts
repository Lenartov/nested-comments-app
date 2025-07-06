import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommentRead, CommentListResponse } from '../../models/comment.model';
import { CommentService } from '../../services/comment.service';
import { CommonModule } from '@angular/common';
import { SafeHtmlPipe } from '../../pipes/safe-html.pipe';
import { CommentSelectionService } from '../../services/comment-selection.service';
import { Subscription } from 'rxjs';
import { FileService } from '../../services/file.service'
import { sortComments } from '../../Utils/sort-comment';
import { CommentConfigService } from '../../services/comment-config.service';

@Component({
  selector: 'app-comment-list',
  imports: [CommonModule, SafeHtmlPipe],
  templateUrl: './comment-list.html',
  styleUrls: ['./comment-list.css'],
  standalone: true,
})
export class CommentList implements OnInit, OnDestroy {
  @Input() currentParentId?: number;
  @Input() comments: CommentRead[] = [];

  expandedComments = new Set<number>();

  currentPage: number = 1;
  sortBy: keyof CommentRead = 'createdAt';
  sortDirection: 'asc' | 'desc' = 'desc';
  
  totalCommentsCount = 0;

  selectedParentId: number | null = null;
  parentIdSub?: Subscription;

  fileModalUrl: string | null = null;

  constructor(private commentService: CommentService, private commentSelection: CommentSelectionService, public fileService: FileService, private config: CommentConfigService) {}

  ngOnInit(): void {
    this.currentPage = this.config.DEFAULT_PAGE;
    this.sortBy = this.config.SORT_BY;
    this.sortDirection = this.config.SORT_DIR;

    this.parentIdSub = this.commentSelection.parentId$.subscribe((id) => {this.selectedParentId = id;});
    this.loadComments();
  }

  ngOnDestroy(): void {
    this.parentIdSub?.unsubscribe();
  }

  private loadComments(): void {
    this.commentService.getComments(this.currentParentId, this.currentPage).subscribe({
      next: (data: CommentListResponse) => {
        this.comments = data.items;
        this.totalCommentsCount = data.totalCount;
        this.expandedComments.clear();
      },
      error: (err) => {
        console.error('Failed to load comments', err);
      },
    });
  }

toggleReplies(id: number): void {
  this.expandedComments.has(id) ? this.expandedComments.delete(id) : this.expandedComments.add(id);
}

  isExpanded(comment: CommentRead): boolean {
    return this.expandedComments.has(comment.id);
  }

  sort(field: keyof CommentRead): void {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } 
    else {
      this.sortBy = field;
      this.sortDirection = 'asc';
    }
    this.comments = sortComments(this.comments, field, this.sortDirection);
  }

  get pages(): number[] {
  return Array(this.totalPages).fill(0).map((_, i) => i + 1);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) 
      return;

    this.currentPage = page;
    this.loadComments();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCommentsCount / this.config.PAGE_SIZE);
  }

  selectParent(id: number, event: MouseEvent): void {
    this.commentSelection.setParentId(id);

    const target = event.currentTarget as HTMLElement;
    if (target) {
      target.classList.add('blink');
      setTimeout(() => target.classList.remove('blink'), 200);
    }
  }

  openFile(filePath: string, fileType?: string): void {
    if (this.fileService.isImage(filePath)) {
      this.fileModalUrl = this.fileService.getFullUrl(filePath);
    } else {
      this.fileService.openFile(filePath);
    }
  }

  closeFile(): void {
    this.fileModalUrl = null;
  }
}
