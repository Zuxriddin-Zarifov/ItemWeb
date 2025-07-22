export class Item {
  id: number;
  name: string;
  type: number;
  date: Date;

  constructor(id: number, name: string, type: number, date: string | Date) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.date = new Date(date);
  }
}