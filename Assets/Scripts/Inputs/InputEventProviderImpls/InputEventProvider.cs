using UniRx;
using UnityEngine;

namespace Inputs.InputEventProviderImpls
{
    public class InputEventProvider : MonoBehaviour, IInputEventProvider
    {
        readonly ReactiveProperty<Vector2> moveDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<Vector2> targetDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<bool> fire = new ReactiveProperty<bool>();
        readonly ReactiveProperty<bool> boost = new ReactiveProperty<bool>();
        readonly ReactiveProperty<bool> reload = new ReactiveProperty<bool>();

        IReadOnlyReactiveProperty<Vector2> IInputEventProvider.MoveDirection => moveDirection;
        IReadOnlyReactiveProperty<Vector2> IInputEventProvider.TargetDirection => targetDirection;
        IReadOnlyReactiveProperty<bool> IInputEventProvider.Fire => fire;
        IReadOnlyReactiveProperty<bool> IInputEventProvider.Boost => boost;
        IReadOnlyReactiveProperty<bool> IInputEventProvider.Reload => reload;

        void Start()
        {
            this.ObserveEveryValueChanged(_ =>
                    new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")))
                .Subscribe(x => moveDirection.Value = x);


            this.ObserveEveryValueChanged(_ => Input.mousePosition)
                .WithLatestFrom(this.ObserveEveryValueChanged(_ => Screen.width)
                        .Select(w => w / 2)
                        .CombineLatest(this.ObserveEveryValueChanged(_ => Screen.height)
                            .Select(h => h / 2),
                            (w, h) => (w, h)),
                    (p, t) => new Vector2(p.x - t.w, p.y - t.h))
                .Subscribe(x => targetDirection.Value = x);

            this.ObserveEveryValueChanged(_ =>
                    Input.GetButton("Fire"))
                .Subscribe(x => fire.Value = x);

            this.ObserveEveryValueChanged(_ =>
                    Input.GetButton("Boost"))
                .Subscribe(x => boost.Value = x);

            this.ObserveEveryValueChanged(_ =>
                    Input.GetButton("Reload"))
                .Subscribe(x => reload.Value = x);
        }
    }
}
