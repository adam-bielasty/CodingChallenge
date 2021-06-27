import { Language } from './Language';
import { Currency } from './Currency';

export class Country {
    name: string;
    flag: string;
    alpha3Code: string;
    population: number;
    timeZones: string[];
    currencies: Currency[];
    languages: Language[];
    capital: string;
    borders: string[];
    countryBorders: string[];
}


