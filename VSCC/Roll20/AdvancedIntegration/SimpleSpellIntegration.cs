namespace VSCC.Roll20.AdvancedIntegration
{
    using System.Collections.ObjectModel;

    public class SimpleSpellIntegration
    {
        public bool ShowSpellDescription { get; set; }

        #region Hit

        public ScalableDie HitDie { get; set; } = new ScalableDie();
        public bool HitIncludeProfficiency { get; set; }
        public bool HitIncludeStr { get; set; }
        public bool HitIncludeDex { get; set; }
        public bool HitIncludeCon { get; set; }
        public bool HitIncludeWis { get; set; }
        public bool HitIncludeCha { get; set; }
        public bool HitIncludeInt { get; set; }
        public bool HitIncludeSpellcastingAbility { get; set; }
        public bool HitIsSpellSave { get; set; }
        public ScalableValue HitConstant { get; set; } = new ScalableValue();
        public ScalableValue SaveConstant { get; set; } = new ScalableValue();
        public string SaveAttr { get; set; }

        #endregion

        #region Damage

        public bool DamageIncludeProfficiency { get; set; }
        public bool DamageIncludeStr { get; set; }
        public bool DamageIncludeDex { get; set; }
        public bool DamageIncludeCon { get; set; }
        public bool DamageIncludeWis { get; set; }
        public bool DamageIncludeCha { get; set; }
        public bool DamageIncludeInt { get; set; }
        public bool DamageIncludeSpellcastingAbility { get; set; }

        public ObservableCollection<ScalableDamageLine> Damage { get; set; } = new ObservableCollection<ScalableDamageLine>();

        #endregion

        public SimpleSpellIntegration Copy()
        {
            SimpleSpellIntegration sii = new SimpleSpellIntegration
            {
                ShowSpellDescription = this.ShowSpellDescription,
                HitDie = this.HitDie.Copy(),
                HitConstant = this.HitConstant.Copy(),
                HitIncludeProfficiency = this.HitIncludeProfficiency,
                HitIncludeStr = this.HitIncludeStr,
                HitIncludeDex = this.HitIncludeDex,
                HitIncludeCon = this.HitIncludeCon,
                HitIncludeWis = this.HitIncludeWis,
                HitIncludeCha = this.HitIncludeCha,
                HitIncludeInt = this.HitIncludeInt,
                HitIncludeSpellcastingAbility = this.HitIncludeSpellcastingAbility,
                HitIsSpellSave = this.HitIsSpellSave,
                DamageIncludeProfficiency = this.DamageIncludeProfficiency,
                DamageIncludeStr = this.DamageIncludeStr,
                DamageIncludeDex = this.DamageIncludeDex,
                DamageIncludeCon = this.DamageIncludeCon,
                DamageIncludeWis = this.DamageIncludeWis,
                DamageIncludeCha = this.DamageIncludeCha,
                DamageIncludeInt = this.DamageIncludeInt,
                DamageIncludeSpellcastingAbility = this.DamageIncludeSpellcastingAbility,
                SaveConstant = this.SaveConstant.Copy(),
                SaveAttr = this.SaveAttr
            };

            foreach (ScalableDamageLine dl in this.Damage)
            {
                sii.Damage.Add(dl.Copy());
            }

            return sii;
        }
    }
}
