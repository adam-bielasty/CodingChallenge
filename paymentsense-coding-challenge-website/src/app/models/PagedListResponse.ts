import { MetaData } from './MetaData';
export class PagedListResponse<T> {
    items: T[];
    metaData: MetaData;
}
