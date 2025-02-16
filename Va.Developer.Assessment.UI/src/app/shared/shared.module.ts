import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FooterComponent } from "./footer/footer.component";
import { NavigationComponent } from "./navigation/navigation.component";

@NgModule({
    declarations: [FooterComponent, NavigationComponent],
    imports: [CommonModule],
    exports: [FooterComponent, NavigationComponent]
})
export class SharedModule {}