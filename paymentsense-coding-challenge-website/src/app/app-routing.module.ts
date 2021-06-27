import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddCountryComponent } from './countries/add-country/add-country.component';
import { CountryListComponent } from './countries/country-list/country-list.component';


const routes: Routes = [
    {
        path: '',
        component: CountryListComponent
    },
    {
        path: 'country/add',
        component: AddCountryComponent
    },
    {
        path: '**',
        component: CountryListComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            routes
        )
    ],
    providers: [],
    exports: [RouterModule]
})

export class AppRoutingModule {
}
