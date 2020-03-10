function [t, z] = EULER(fun, u, z0, N, h)

t = (0:h:N*h-h);
z = zeros(length(z0), N);
z(:,1) = z0;    % initial condidtions

for k = 1:N-1  
    
    z(:,k+1) = z(:,k) + h*fun(u(:,k), z(:,k)); 
end

end

