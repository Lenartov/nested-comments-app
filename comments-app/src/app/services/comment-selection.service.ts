
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CommentSelectionService {
  private parentIdSubject = new BehaviorSubject<number | null>(null);
  parentId$ = this.parentIdSubject.asObservable();

  setParentId(id: number | null) {
    this.parentIdSubject.next(id);
  }

  clearParentId() {
  this.parentIdSubject.next(null);
}
}
