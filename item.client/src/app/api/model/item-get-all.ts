import { Item } from "../../services/models/item";

export class ItemGetAll {    
  total!: number;
  currentPage!: number;
  pageSize!: number;
  totalPages!: number;
  items!: Item[];

  constructor(data: any) {
    this.total = data.total;
    this.currentPage = data.currentPage;
    this.pageSize = data.pageSize;
    this.totalPages = data.totalPages;
    this.items = data.items;
  }
}