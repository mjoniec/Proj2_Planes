import { Component, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { combineLatest } from 'rxjs';
import { takeWhile } from 'rxjs/operators';
import { NbThemeService } from '@nebular/theme';
import { registerMap } from 'echarts';

@Component({
  selector: 'ngx-bubble-map',
  styleUrls: ['./bubble-map.component.scss'],
  template: `
    <nb-card>
      <nb-card-header>Bubble Maps</nb-card-header>
      <div echarts [options]="options" class="echarts"></div>
    </nb-card>
  `,
})
export class BubbleMapComponent implements OnDestroy {

  latlong: any = {};
  mapData: any[];
  max = -Infinity;
  min = Infinity;
  options: any;

  bubbleTheme: any;
  geoColors: any[];

  private alive = true;

  constructor(private theme: NbThemeService,
              private http: HttpClient) {

    combineLatest([
      this.http.get('assets/map/world.json'),
      this.theme.getJsTheme(),
    ])
      .pipe(takeWhile(() => this.alive))
      .subscribe(([map, config]: [any, any]) => {

        registerMap('world', map);

        const colors = config.variables;
        this.bubbleTheme = config.variables.bubbleMap;
        this.geoColors = [colors.primary, colors.info, colors.success, colors.warning, colors.danger];

        this.mapData = [
          { 'latitude': 23, 'longitude': 55, 'name': 'yyy', 'value': 200, 'color': this.getRandomGeoColor() },
          { 'latitude': 11, 'longitude': 10, 'name': 'xxx', 'value': 100, 'color': this.getRandomGeoColor() }];

        this.mapData.forEach((itemOpt) => {
          if (itemOpt.value > this.max) {
            this.max = itemOpt.value;
          }
          if (itemOpt.value < this.min) {
            this.min = itemOpt.value;
          }
        });

        this.options = {
          tooltip: {
            trigger: 'item',
            formatter: params => {
              return `${params.name}: ${params.value[2]}`;
            },
          },
          visualMap: {
            show: false,
            min: 0,
            max: this.max,
            inRange: {
              symbolSize: [3, 30],
            },
          },
          geo: {
            name: 'World Population (2010)',
            type: 'map',
            map: 'world',
            roam: true,
            label: {
              emphasis: {
                show: false,
              },
            },
            itemStyle: {
              normal: {
                areaColor: this.bubbleTheme.areaColor,
                borderColor: this.bubbleTheme.areaBorderColor,
              },
              emphasis: {
                areaColor: this.bubbleTheme.areaHoverColor,
              },
            },
            zoom: 1.1,
          },
          series: [
            {
              type: 'scatter',
              coordinateSystem: 'geo',
              data: this.mapData.map(itemOpt => {
                return {
                  name: itemOpt.name,
                  value: [
                    itemOpt.longitude,
                    itemOpt.latitude,
                    itemOpt.value,
                  ],
                  itemStyle: {
                    normal: {
                      color: itemOpt.color,
                    },
                  },
                };
              }),
            },
          ],
        };
      });
  }

  ngOnDestroy() {
    this.alive = false;
  }

  private getRandomGeoColor() {
    const index = Math.round(Math.random() * this.geoColors.length);
    return this.geoColors[index];
  }
}
