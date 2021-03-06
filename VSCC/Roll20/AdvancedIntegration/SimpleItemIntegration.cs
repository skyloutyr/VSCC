﻿namespace VSCC.Roll20.AdvancedIntegration
{
    using System.Collections.ObjectModel;

    public class SimpleItemIntegration
    {
        #region Hit

        public int HitDieSide { get; set; }
        public bool HitIncludeProfficiency { get; set; }
        public bool HitIncludeStr { get; set; }
        public bool HitIncludeDex { get; set; }
        public bool HitIncludeCon { get; set; }
        public bool HitIncludeWis { get; set; }
        public bool HitIncludeCha { get; set; }
        public bool HitIncludeInt { get; set; }
        public int HitConstant { get; set; }

        #endregion

        #region Damage

        public bool DamageIncludeProfficiency { get; set; }
        public bool DamageIncludeStr { get; set; }
        public bool DamageIncludeDex { get; set; }
        public bool DamageIncludeCon { get; set; }
        public bool DamageIncludeWis { get; set; }
        public bool DamageIncludeCha { get; set; }
        public bool DamageIncludeInt { get; set; }

        public ObservableCollection<DamageLine> Damage { get; set; } = new ObservableCollection<DamageLine>();

        #endregion

        public SimpleItemIntegration Copy()
        {
            SimpleItemIntegration sii = new SimpleItemIntegration
            {
                HitDieSide = this.HitDieSide,
                HitConstant = this.HitConstant,
                HitIncludeProfficiency = this.HitIncludeProfficiency,
                HitIncludeStr = this.HitIncludeStr,
                HitIncludeDex = this.HitIncludeDex,
                HitIncludeCon = this.HitIncludeCon,
                HitIncludeWis = this.HitIncludeWis,
                HitIncludeCha = this.HitIncludeCha,
                HitIncludeInt = this.HitIncludeInt,
                DamageIncludeProfficiency = this.DamageIncludeProfficiency,
                DamageIncludeStr = this.DamageIncludeStr,
                DamageIncludeDex = this.DamageIncludeDex,
                DamageIncludeCon = this.DamageIncludeCon,
                DamageIncludeWis = this.DamageIncludeWis,
                DamageIncludeCha = this.DamageIncludeCha,
                DamageIncludeInt = this.DamageIncludeInt
            };

            foreach (DamageLine dl in this.Damage)
            {
                sii.Damage.Add(dl.Copy());
            }

            return sii;
        }
    }
}
