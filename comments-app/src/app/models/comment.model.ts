export interface CommentCreate {
  userName: string;           
  email: string;              
  homePage?: string | null;   
  captcha: string;            
  message: string;            
  parentCommentId?: number | null; 
}

export interface CommentRead {
  id: number;
  userName: string;
  email : string;
  message: string;
  createdAt: string;      
  homePage?: string;
  filePath?: string;
  fileExtension?: string;
  hasReplies : boolean;
}

export interface CommentListResponse {
  items: CommentRead[];
  totalCount: number;
}